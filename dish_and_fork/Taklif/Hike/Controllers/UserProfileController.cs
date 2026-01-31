using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly UserManager<HikeUser> _userManager;
        private readonly SignInManager<HikeUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public UserProfileController(HikeDbContext db,
            UserManager<HikeUser> userManager,
            SignInManager<HikeUser> signInManager,
            IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Получить данные текущего пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user-profile/my")]
        [ProducesResponseType(200, Type = typeof(UserProfileDetailsReadModel))]
        public async Task<UserProfileDetailsReadModel> GetSelf()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (profile == null)
                return null;
            return await Get(profile.Id);
        }

        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpGet("admin/user-profile/{id}")]
        [ProducesResponseType(200, Type = typeof(UserProfileDetailsReadModel))]
        public async Task<UserProfileDetailsReadModel> Get([FromRoute, Required] string id)
        {
            var profile = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (profile == null)
                return null;
            var user = await _userManager.FindByIdAsync(profile.Id);
            if (user == null)
                return null;
            var currentLogins = await _userManager.GetLoginsAsync(user);
            var ur = await _db.UserRoles.Where(x => x.UserId == profile.Id).Select(x => x.RoleId).ToListAsync();
            var roles = await _db.Roles.Where(x => ur.Contains(x.Id)).ToListAsync();
            var hasPassword = await _userManager.HasPasswordAsync(user);
            var otherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
               .Where(auth => currentLogins.All(ul => auth.Name != ul.LoginProvider))
               .ToList();
            return UserProfileDetailsReadModel.From(profile, roles, currentLogins, hasPassword, otherLogins.Select(l => AuthenticationSchemeReadModel.From(l)));
        }

        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpPost("admin/user-profile/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<UserProfileReadModel>))]
        public async Task<PageResultModel<UserProfileReadModel>> Filter(UserProfileFilterModel model)
        {
            var dtos = await _db.Users
                .AsNoTracking()
                .OrderBy(x => x.NormalizedUserName)
                .Take((int)model.PageSize)
                .Skip((int)model.Skip())
                .ToListAsync();
            var totalCount = await _db.Users.CountAsync();
            var result = dtos.Select(x => UserProfileReadModel.From(x)).Where(x => x != null).ToList();
            return new PageResultModel<UserProfileReadModel>()
            {
                Items = result,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Добавить пользователю роль
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("admin/user-profile/{id}/role/{roleId}")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> AddRole([FromRoute, Required] string id, [FromRoute, Required] string roleId)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("Не удалось найти пользователя")
                {
                    Data = { ["id"] = id }
                };
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null)
                throw new ApplicationException("Не удалось найти роль")
                {
                    Data = { ["id"] = roleId }
                };
            var ur = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == roleId);
            if (ur != null)
                return 0;
            var nr = new IdentityUserRole<string> { UserId = user.Id, RoleId = roleId };
            _db.UserRoles.Add(nr);
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Забрать у пользователя роль
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("admin/user-profile/{id}/role/{roleId}")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> RemoveRole([FromRoute, Required] string id, [FromRoute, Required] string roleId)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("Не удалось найти пользователя")
                {
                    Data = { ["id"] = id }
                };
            var ur = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id && x.RoleId == roleId);
            if (ur == null)
                return 0;
            _db.UserRoles.Remove(ur);
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить профиль пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("user-profile/my")]
        [ProducesResponseType(200)]
        public async Task UpdateSelf(UserProfileUpdateModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { model, id } }
                };
            user.UserName = model.UserName;
            user.NormalizedUserName = model.UserName?.ToUpper();
            if (model.AcceptedConsentToMailings && !user.AcceptedConsentToMailings)
            {
                user.AcceptedConsentToMailings = true;
                var claim = User.FindFirst(nameof(HikeUser.AcceptedConsentToMailings));
                await _userManager.ReplaceClaimAsync(
                    user,
                    claim,
                    new Claim(nameof(HikeUser.AcceptedConsentToMailings), user.AcceptedConsentToMailings.ToString())
                    );
            }
            if (model.AcceptedConsentToPersonalDataProcessing && !user.AcceptedConsentToPersonalDataProcessing)
            {
                user.AcceptedConsentToPersonalDataProcessing = true;
                var claim = User.FindFirst(nameof(HikeUser.AcceptedConsentToPersonalDataProcessing));
                await _userManager.ReplaceClaimAsync(
                    user,
                    claim,
                    new Claim(nameof(HikeUser.AcceptedConsentToPersonalDataProcessing), user.AcceptedConsentToPersonalDataProcessing.ToString())
                    );
            }
            if (model.AcceptedPivacyPolicy && !user.AcceptedPivacyPolicy)
            {
                user.AcceptedPivacyPolicy = true;
                var claim = User.FindFirst(nameof(HikeUser.AcceptedPivacyPolicy));
                await _userManager.ReplaceClaimAsync(
                    user,
                    claim,
                    new Claim(nameof(HikeUser.AcceptedPivacyPolicy), user.AcceptedPivacyPolicy.ToString())
                    );
            }
            if (model.AcceptedTermsOfUse && !user.AcceptedTermsOfUse)
            {
                user.AcceptedTermsOfUse = true;
                var claim = User.FindFirst(nameof(HikeUser.AcceptedTermsOfUse));
                await _userManager.ReplaceClaimAsync(
                    user,
                    claim,
                    new Claim(nameof(HikeUser.AcceptedTermsOfUse), user.AcceptedTermsOfUse.ToString())
                    );
            }
            if (model.AcceptedOfferFoUser && !user.AcceptedOfferFoUser)
            {
                user.AcceptedOfferFoUser = true;
                var claim = User.FindFirst(nameof(HikeUser.AcceptedOfferFoUser));
                await _userManager.ReplaceClaimAsync(
                    user,
                    claim,
                    new Claim(nameof(HikeUser.AcceptedOfferFoUser), user.AcceptedOfferFoUser.ToString())
                    );
            }
            await _db.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
        }

        /// <summary>
        /// Создать пароль для пользователя.
        /// Это нужно когда пользователь зарегестрировался через Google или другого внешнего провайдера.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("user-profile/my/password")]
        [ProducesResponseType(200)]
        public async Task SetPassword(PasswordCreateModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { userId = _userManager.GetUserId(User) } }
                };
            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
                throw new ApplicationException("Ошибка при смене пароля")
                {
                    Data = { ["args"] = new { userId = _userManager.GetUserId(User), addPasswordResult } }
                };

            await _signInManager.RefreshSignInAsync(user);
        }

        /// <summary>
        /// Сменить пароль текущего пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("user-profile/my/password")]
        [ProducesResponseType(200)]
        public async Task UpdatePassword(PasswordUpdateModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { userId = _userManager.GetUserId(User) } }
                };
            var addPasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!addPasswordResult.Succeeded)
                throw new ApplicationException("Ошибка при смене пароля")
                {
                    Data = { ["args"] = new { userId = _userManager.GetUserId(User), addPasswordResult } }
                };

            await _signInManager.RefreshSignInAsync(user);
        }

        /// <summary>
        /// Сменить email текущего пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("user-profile/my/email")]
        [ProducesResponseType(200)]
        public async Task UpdateEmail(EmailUpdateModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { userId = _userManager.GetUserId(User) } }
                };
            var email = await _userManager.GetEmailAsync(user);
            if (model.NewEmail?.GetNormalizedName() == email?.GetNormalizedName())
                return;
            var old = await _userManager.FindByEmailAsync(model.NewEmail);
            if (old != null)
                throw new ApplicationException("Пользователь с такой почтой уже зарегестрирован")
                {
                    Data = { ["args"] = model }
                };
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new { userId = userId, email = model.NewEmail, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                model.NewEmail,
                "Подтвердите свою почту",
                $"Для подтверждения своей почты пожалуйста <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите сюда</a>.");
        }

        /// <summary>
        /// Удалить логин
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("user-profile/my/external-login")]
        [ProducesResponseType(200)]
        public async Task RemoveLogin(LoginRemoveModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { userId = id, model } }
                };

            var currentLogins = await _userManager.GetLoginsAsync(user);
            var showRemoveButton = !string.IsNullOrWhiteSpace(user.PasswordHash) || currentLogins.Count > 1;
            if (!showRemoveButton)
                throw new ApplicationException("Эту соц сеть нельзя удалять. Сначало либо установите пароль на свой аккаунт либо добавье еще одну соцсеть.")
                {
                    Data = { ["args"] = new { userId = id, user, model } }
                };
            var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
                throw new ApplicationException("Не удалось удалить соц сеть.")
                {
                    Data = { ["args"] = new { userId = id, user, model, result } }
                };

            await _signInManager.RefreshSignInAsync(user);
        }

        /// <summary>
        /// Добавить логин
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("user-profile/my/external-login")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<string> AddLogin(AddLoginModel model)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var redirectUrl = $"{this.Request.Scheme}://{this.Request.Host}/Identity/Account/Manage/ExternalLogins?handler=LinkLoginCallback";// Url.Action(nameof(AddLoginCallback), nameof(UserProfileController));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(model.LoginProvider, redirectUrl, id);
            await HttpContext.ChallengeAsync(model.LoginProvider, properties);
            var location = HttpContext.Response.Headers["Location"].First();
            Response.StatusCode = 200;
            HttpContext.Response.Headers.Remove("Location");
            return location;
        }

        /// <summary>
        /// Колбек для подтверждения добавления логина
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("user-profile/my/external-login")]
        [ProducesResponseType(200)]
        public async Task AddLoginCallback()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ApplicationException("User not found")
                {
                    Data = { ["args"] = new { userId = id } }
                };

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
                throw new ApplicationException("Логины не найдены")
                {
                    Data = { ["args"] = new { userId = id } }
                };

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
                throw new ApplicationException("Соцсеть не была добавлена. Соцсеть может быть ассоциирована только с одним аккаунтом.")
                {
                    Data = { ["args"] = new { userId = id, result } }
                };
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }
}
