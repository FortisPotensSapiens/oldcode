using System.Security.Claims;
using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{

    [Route("api/v1")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly UserManager<HikeUser> _userManager;
        private readonly ITwilioClient _twilio;
        private readonly IEmailClient _email;

        public PartnersController(
            HikeDbContext db,
            SignInManager<HikeUser> signInManager,
            UserManager<HikeUser> userManager,
            ITwilioClient twilio,
            IEmailClient email
            )
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _twilio = twilio;
            _email = email;
        }

        /// <summary>
        /// Получить список партнеров для покупателя
        /// </summary>
        /// <returns></returns>
        [HttpGet("partners")]
        [ProducesResponseType(200, Type = typeof(List<PartnerReadModel>))]
        public async Task<List<PartnerReadModel>> GetAll()
        {
            var dtos = await _db.Shops
                .AsNoTracking()
                .Where(x => x.Partner.State == PartnerState.Confirmed)
               .Include(x => x.Partner)
                .ThenInclude(x => x.Image)
                .ToListAsync();
            return dtos.Select(x => PartnerReadModel.From(x, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}")).ToList();
        }

        /// <summary>
        /// Получить данные о партнере
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("partners/{id}")]
        [ProducesResponseType(200, Type = typeof(PartnerReadModel))]
        public async Task<PartnerReadModel> Get(Guid id)
        {
            var dto = await _db.Shops
                .AsNoTracking()
                 .Include(x => x.Partner)
                .ThenInclude(x => x.Image)
                .FirstOrDefaultAsync(x => x.Id == id);
            return PartnerReadModel.From(dto, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        }

        /// <summary>
        /// Получить данные о моем партнере
        /// </summary>
        /// <returns></returns>
        [HttpGet("partners/my")]
        [ProducesResponseType(200, Type = typeof(PartnerReadModel))]
        [Authorize]
        public async Task<PartnerReadModel> GetMy()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = await _db.Shops
                .AsNoTracking()
                .Include(x => x.Partner)
                .ThenInclude(x => x.Image)
                .FirstOrDefaultAsync(x => x.Partner.Employes.Any(x => x.UserId == userId));
            return PartnerReadModel.From(dto, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        }

        /// <summary>
        /// Создать нового партнера в системе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [HttpPost("partners")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(PartnerCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using var tran = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var old = await _db.Partners.AsNoTracking().FirstOrDefaultAsync(x => x.Employes.Any(x => x.UserId == userId));
            if (old != null)
                throw new ApplicationException("У вас уже есть созданный профиль продавца!");
            if (User.IsInRole("seller"))
                throw new ApplicationException("Вы уже зарегестрированы как продавец");
            var profile = await _db.Users.FirstOrDefaultAsync(p => p.Id == userId);
            if (profile == null)
                throw new ApplicationException("Не найден профиль для текущего пользователя")
                {
                    Data = { ["userId"] = userId }
                };
            var dto = model.ToPartnerDto(profile.Id);
            if (await _db.Partners.AnyAsync(p => p.Inn == dto.Inn))
                throw new ApplicationException("Магазин с таким ИНН уже существует!")
                {
                    Data = { ["args"] = model }
                };
            if (string.IsNullOrWhiteSpace(dto.ContactEmail))
                dto.ContactEmail = profile.Email;
            var virfied = await _twilio.CheckVerificationCode(model.ContactPhone, model.PhoneComfinmationCode);
            if (virfied != VerivicationStatus.Approved)
                new { model }.ThrowApplicationException("Не верный код подтверждения!");
            _db.Partners.Add(dto);
            var shop = new ShopDto
            {
                Id = dto.Id,
                PartnerId = dto.Id
            };
            _db.Shops.Add(shop);
            await _db.SaveChangesAsync();
            var sellerRole = await _db.Roles.AsNoTracking()
            .FirstOrDefaultAsync(x => x.NormalizedName == "seller".GetNormalizedName());
            if (!(await _db.UserRoles.AnyAsync(x => x.RoleId == sellerRole.Id && x.UserId == profile.Id)))
            {
                _db.UserRoles.Add(
                    new IdentityUserRole<string>() { RoleId = sellerRole.Id, UserId = profile.Id });
                await _signInManager.RefreshSignInAsync(profile);
                await _userManager.AddClaimAsync(profile, new Claim(ClaimTypes.Role, sellerRole.Name));
                await _userManager.AddClaimAsync(profile, new Claim("role", sellerRole.Name));
            }
            await _db.SaveChangesAsync();
            var admins = await _userManager.GetUsersInRoleAsync("admin");
            await tran.CommitAsync();
            foreach (var user in admins.Where(x => !string.IsNullOrWhiteSpace(x.Email)))
            {
                await _email.SendEmailAsync(
                    user.Email,
                    $"Партнёр {model.Title} c ИНН {model.Inn} подал заявку на сотрудничество!",
                    $"<div><h1>Партнёр {model.Title} c ИНН {model.Inn} подал заявку на сотрудничество!</div></h1>"
                    );
            }
            return dto.Id;
        }

        /// <summary>
        /// Обновить свободные данные для партнера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("seller/partners")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> UpdatePartnerInfo(PartnerUpdateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var partner = await _db.Partners
                .Include(x => x.Employes)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.Employes.Any(e => e.UserId == userId));
            if (partner == null)
                throw new ApplicationException("Не удалось найти партнера")
                {
                    Data = { ["data"] = new { userId, model } }
                };
            if (model.ContactPhone != partner.ContactPhone)
            {
                if (string.IsNullOrWhiteSpace(model.PhoneComfinmationCode))
                    new { model }.ThrowApplicationException("Укажите код подтверждения из SMS если меняете номер телефона!");
                var virfied = await _twilio.CheckVerificationCode(model.ContactPhone, model.PhoneComfinmationCode);
                if (virfied != VerivicationStatus.Approved)
                    new { model }.ThrowApplicationException("Не верный код подтверждения!");
            }
            model.Map(partner);

            if (model.Address != null)
            {
                var address = model.Address.ToAddress();
                partner.Address = address;
                //if (partner.Address == null)
                //{
                //    var address = model.Address.ToAddress();
                //    partner.Address = address;
                //}
                //else
                //{
                //    model.Address.Map(partner.Address);
                //}
            }
            else
            {
                partner.Address = null;
            }
            if (model.RegistrationAddress != null)
            {
                var address = model.RegistrationAddress.ToAddress();
                partner.RegistrationAddress = address;
                //if (partner.RegistrationAddress == null)
                //{
                //    var address = model.RegistrationAddress.ToAddress();
                //    partner.RegistrationAddress = address;
                //}
                //else
                //{
                //    model.RegistrationAddress.Map(partner.RegistrationAddress);
                //}
            }
            else
            {
                partner.RegistrationAddress = null;
            }
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Подтвердить партнера.
        /// Это даст ему возможность размещать товары на сайте.
        /// К тому же это дасть директору этого магазина роль seller
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("admin/partners/confirm/{id}")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> ConfirmPartner([FromRoute] Guid id)
        {
            var partner = await _db.Partners
                .Include(x => x.Employes)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (partner == null)
                throw new ApplicationException("Не удалось найти партнера");
            partner.State = PartnerState.Confirmed;
            var sellerRole = await _db.Roles.AsNoTracking()
                .FirstOrDefaultAsync(x => x.NormalizedName == "seller".GetNormalizedName());
            var adminRole = await _db.Roles.AsNoTracking()
          .FirstOrDefaultAsync(x => x.NormalizedName == "admin".GetNormalizedName());
            var eid = partner.Employes.Select(x => x.UserId).ToList();
            if (await _db.UserRoles.AnyAsync(r => eid.Contains(r.UserId) && r.RoleId == adminRole.Id))
                new { id }.ThrowApplicationException("Один из работников этого партнера уже обладает ролью админа! Нельзя быть одновременно админом и продавцом!");
            foreach (var employe in partner.Employes)
            {
                if ((await _db.UserRoles.AnyAsync(x => x.RoleId == sellerRole.Id && x.UserId == employe.UserId)))
                    continue;
                _db.UserRoles.Add(
                    new IdentityUserRole<string>() { RoleId = sellerRole.Id, UserId = employe.UserId });
                var user = await _db.Users.FindAsync(employe.UserId);
                await _signInManager.RefreshSignInAsync(user);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, sellerRole.Name));
                await _userManager.AddClaimAsync(user, new Claim("role", sellerRole.Name));
            }
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Запблокировать партнера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpPost("admin/partners/{id}/block")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> BlockPartner([FromRoute] Guid id)
        {
            var partner = await _db.Partners
                .FirstOrDefaultAsync(x => x.Id == id);
            if (partner == null)
                throw new ApplicationException("Не удалось найти партнера");
            partner.State = PartnerState.Blocked;
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Разблокировать партнера во имя Кхорна и Тзинча
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpPost("admin/partners/{id}/unblock")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> UnblockPartner([FromRoute] Guid id)
        {
            var partner = await _db.Partners
                .FirstOrDefaultAsync(x => x.Id == id);
            if (partner == null)
                throw new ApplicationException("Не удалось найти партнера");
            partner.State = PartnerState.Confirmed;
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Получить список партнеров для админа
        /// </summary>
        /// <returns></returns>
        [HttpGet("admim/partners")]
        [ProducesResponseType(200, Type = typeof(List<PartnerReadModel>))]
        public async Task<List<PartnerReadModel>> GetAllForAdmin()
        {
            var dtos = await _db.Shops
                .AsNoTracking()
               .Include(x => x.Partner)
                .ThenInclude(x => x.Image)
                .ToListAsync();
            return dtos.Select(x => PartnerReadModel.From(x, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}")).ToList();
        }

        /// <summary>
        /// Установить внешний id 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="externalId"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpPut("admin/partners/{id}/set-external-id/{externalId}")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> SetExternalId([FromRoute, Required] Guid id, [FromRoute, Required] string externalId)
        {
            var partner = await _db.Partners
                .FirstOrDefaultAsync(x => x.Id == id);
            if (partner == null)
                throw new ApplicationException("Не удалось найти партнера");
            if (string.IsNullOrWhiteSpace(externalId))
                new { id }.ThrowApplicationException("Укажите внешний id партнера (в платежке)");
            partner.ExternalId = externalId?.Trim();
            return await _db.SaveChangesAsync();
        }
    }
}
