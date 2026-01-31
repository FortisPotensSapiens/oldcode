using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Attributes;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Hike.Controllers
{
    /// <summary>
    /// Работа с индивидуальными заказами
    /// </summary>
    [Route("api/v1")]
    [ApiController]
    public class IndividualOrdersController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly ICancellationTokensRepository _cancellationTokens;
        private readonly IWebSocketsClient _webSockets;
        private readonly ILogger<IndividualOrdersController> _logger;
        private readonly IPushNotificationsClient _pushNotifications;
        private readonly IBaseUriRepository _baseUris;

        public IndividualOrdersController(
            HikeDbContext db,
            ICancellationTokensRepository cancellationTokens,
            IWebSocketsClient webSockets,
            ILogger<IndividualOrdersController> logger,
            IPushNotificationsClient pushNotifications,
            IBaseUriRepository baseUris
            )
        {
            _db = db;
            _cancellationTokens = cancellationTokens;
            _webSockets = webSockets;
            _logger = logger;
            _pushNotifications = pushNotifications;
            _baseUris = baseUris;
        }

        /// <summary>
        /// Создание заявки на индивидуальный заказ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("individual-applications")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(ApplicationCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (profile == null)
                throw new ApplicationException("Профиль пользователя не найден для заказчика") { Data = { ["userId"] = userId, ["model"] = model } };
            var dto = model.ToIndividualOrder(profile.Id);
            _db.Applications.Add(dto);
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
            return dto.Id;
        }

        /// <summary>
        /// Получение списка моих заявок (созданных мной как покупателем)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("individual-applications")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<ApplicationReadModel>))]
        public async Task<PageResultModel<ApplicationReadModel>> GetMy([FromQuery] PaginationModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (profile == null)
                throw new ApplicationException("Профиль пользователя не найден для заказчика") { Data = { ["userId"] = userId, ["model"] = model } };
            var result = await _db.Applications
                .AsNoTracking()
                .Where(x => x.CustomerId == profile.Id)
                .OrderByDescending(x => x.Created)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { App = x, Offer = x.Offers.FirstOrDefault(o => o.OrderId != null) })
                .ToListAsync(_cancellationTokens.GetDefault());
            var count = await _db.Applications
                .Where(x => x.CustomerId == profile.Id)
                 .OrderByDescending(x => x.Created)
                .CountAsync(_cancellationTokens.GetDefault());
            return new PageResultModel<ApplicationReadModel>
            {
                Items = result.Select(x => ApplicationReadModel.From(x.App, x.Offer)).ToList(),
                TotalCount = count
            };
        }

        /// <summary>
        /// Получение списка купленный заказов (по продовцам чтобы показывать в инфо о продавце какие индивидуальные заказы у него купили)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("individual-applications/buyed/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<ApplicationReadModel>))]
        public async Task<PageResultModel<ApplicationReadModel>> GetBuyed(FiterBuyedApplicationsModel model)
        {
            var filter = GetBuyedApplicationsFilter(model);
            var result = await _db.Applications
                .AsNoTracking()
                .Where(filter)
                .OrderByDescending(x => x.Created)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                 .Select(x => new { App = x, Offer = x.Offers.FirstOrDefault(o => o.OrderId != null) })
                .ToListAsync(_cancellationTokens.GetDefault());
            var count = await _db.Applications
                .Where(filter)
                .CountAsync(_cancellationTokens.GetDefault());
            return new PageResultModel<ApplicationReadModel>
            {
                Items = result.Select(x => ApplicationReadModel.From(x.App, x.Offer)).ToList(),
                TotalCount = count
            };
        }

        private static Expression<Func<ApplicationDto, bool>> GetBuyedApplicationsFilter(FiterBuyedApplicationsModel model)
        {
            if (!model.PartnerId.HasValue)
                return x => x.Offers.Any(a => a.OrderItems.Any(b => b.Order.State != Entities.OrderState.Created));
            return x => x.Offers.Any(a => a.ShopId == model.PartnerId.Value && a.OrderItems.Any(b => b.Order.State != Entities.OrderState.Created));
        }

        public class FiterBuyedApplicationsModel : PaginationModel
        {
            public Guid? PartnerId { get; set; }
        }

        /// <summary>
        /// Получить подробную информацию о заявке по ее Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("individual-applications/{id}")]
        [ProducesResponseType(200, Type = typeof(ApplicationDetailsReadModel))]
        public async Task<ApplicationDetailsReadModel> GetById([FromRoute] Guid id)
        {
            var dto = await _db.Applications
                .Include(x => x.Offers)
                .ThenInclude(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Offers)
                .ThenInclude(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == id, _cancellationTokens.GetDefault());
            return ApplicationDetailsReadModel.From(dto, _baseUris.Get().AbsoluteUri);
        }

        /// <summary>
        /// Получение подробной информации об откиле на заказ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("individual-applications/offers/{id}")]
        [ProducesResponseType(200, Type = typeof(OfferDetailsReamModel))]
        public async Task<OfferDetailsReamModel> GetOfferById([FromRoute] Guid id)
        {
            var dto = await _db.ApplicationOffers
                .Include(x => x.Comments)
                .ThenInclude(x => x.UserProfile)
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .FirstOrDefaultAsync(x => x.Id == id, _cancellationTokens.GetDefault());
            return OfferDetailsReamModel.From(dto, _baseUris.Get().AbsoluteUri);
        }

        /// <summary>
        /// Создать отклик на заявку
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("individual-applications/offers")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        [My(MyAttribute.SELLER)]
        public async Task<Guid> CreateOffer(OfferCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, _cancellationTokens.GetDefault());
            if (profile == null)
                throw new ApplicationException("Профиль пользователя не найден для исполнителя")
                {
                    Data = { ["userId"] = userId, ["model"] = model }
                };
            var partner = await _db.Shops.FirstOrDefaultAsync(x => x.Partner.Employes.Any(y => y.UserId == profile.Id), _cancellationTokens.GetDefault());
            if (partner == null)
                throw new ApplicationException("Не найден магазин исполнителя")
                {
                    Data = { ["userId"] = userId, ["model"] = model }
                };
            var application = await _db.Applications.FirstOrDefaultAsync(x => x.Id == model.ApplicationId,
                _cancellationTokens.GetDefault());
            if (application == null)
                throw new ApplicationException("Не удалось найти заявку!")
                {
                    Data = { ["userId"] = userId, ["model"] = model }
                };
            if (await _db.Applications.AnyAsync(x => x.Id == model.ApplicationId && x.Offers.Any(f => f.OrderId != null)))
                new { userId, model }.ThrowApplicationException("У этого обьявления уже есть купленный отклик! На него нельзя больше откликаться!");
            var dto = model.ToOffer(profile.Id, partner.Id);
            if ((application.ToDate.HasValue && dto.Date.Date > application.ToDate.Value.Date) ||
                (application.FromDate.HasValue && dto.Date.Date < application.FromDate.Value.Date))
                throw new ApplicationException("Дата отклина должна быть в диапазоне дат заявки!")
                {
                    Data = { ["userId"] = userId, ["model"] = model, ["application"] = application }
                };        
            _db.ApplicationOffers.Add(dto);
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
            return dto.Id;
        }

        /// <summary>
        /// Получение списка заявок доступных для отклика
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpGet("seller/individual-applications")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<ApplicationReadModel>))]
        public async Task<PageResultModel<ApplicationReadModel>> GetApplications([FromQuery] PaginationModel model)
        {
            var result = await _db.Applications
                .AsNoTracking()
                .Where(x => !x.Offers.Any(o => o.OrderId != null))
                .OrderByDescending(x => x.Created)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { App = x, Offer = x.Offers.FirstOrDefault(o => o.OrderId != null) })
                .ToListAsync(_cancellationTokens.GetDefault());
            var count = await _db.Applications
                  .Where(x => !x.Offers.Any(o => o.OrderId != null))
                .CountAsync(_cancellationTokens.GetDefault());
            return new PageResultModel<ApplicationReadModel>
            {
                Items = result.Select(x => ApplicationReadModel.From(x.App, x.Offer)).ToList(),
                TotalCount = count
            };
        }

        /// <summary>
        /// Создать коментарий к отклику
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("individual-applications/offers/comments")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateOfferComment(OfferCommentCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, _cancellationTokens.GetDefault());
            if (profile == null)
                throw new ApplicationException("Профиль пользователя не найден для исполнителя")
                {
                    Data = { ["userId"] = userId, ["model"] = model }
                };
            var offer = await _db.ApplicationOffers.Include(x => x.Application).FirstOrDefaultAsync(x => x.Id == model.OfferId,
                _cancellationTokens.GetDefault());
            if (offer == null)
                throw new ApplicationException("Не удалось найти отклик на заявку!")
                {
                    Data = { ["userId"] = userId, ["model"] = model }
                };
            var dto = model.ToComment(profile.Id);
            _db.Comments.Add(dto);
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
            try
            {
                var comment = await _db.Comments.AsNoTracking()
                    .Include(x => x.UserProfile)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);
                var users = await _db.Users
                     .Where(x => x.Id == offer.Application.CustomerId || x.Partners.Any(p => p.PartnerId == offer.ShopId))
                     .Where(x => x.Id != dto.UserProfileId)
                     .AsNoTracking()
                     .ToListAsync();
                await _webSockets.OfferCommentAdded(profile.Id, OfferCommentReadModel.From(comment));
                foreach (var user in users)
                {
                    await _webSockets.OfferCommentAdded(user.Id, OfferCommentReadModel.From(comment));
                    await _pushNotifications.SendAsync(user.Id, new FcmRequest { Message = new FcmMessage { Notification = new FcmNotification { Title = $"Новый коментарий к отклику номер {offer.Number} для заявки номер: {offer.Application.Number}!", Body = dto.Text } } });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при отправке уведомлений");
            }
            return dto.Id;
        }


        /// <summary>
        /// Список откликов который создал я. Откикликов на индивидумальный заказ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPost("seller/individual-applications/offers/my/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<OfferSellerReadModel>))]
        public async Task<PageResultModel<OfferSellerReadModel>> GetMyoffers(PaginationModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _db.ApplicationOffers
                .Where(x => x.Creator.Id == userId)
                .OrderByDescending(x => x.Created)
                .Include(x => x.Application)
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .AsNoTracking()
               .Skip((int)model.Skip())
               .Take((int)model.PageSize)
               .ToListAsync(_cancellationTokens.GetDefault());
            var count = await _db.ApplicationOffers
                .Where(x => x.Creator.Id == userId)
                .OrderByDescending(x => x.Created)
                .CountAsync(_cancellationTokens.GetDefault());
            return new PageResultModel<OfferSellerReadModel>
            {
                Items = result.Select(OfferSellerReadModel.From).ToList(),
                TotalCount = count
            };
        }

        /// <summary>
        /// Удалить заявку вместо со всеми ее откликами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [HttpDelete("individual-applications/{id}")]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> RemoveApplication(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var app = await _db.Applications
                .Include(x => x.Offers)
                .FirstOrDefaultAsync(x => x.Id == id && x.Customer.Id == userId);
            if (app == null)
                throw new ApplicationException("Не удалость найти заявку у этого покупателя!")
                {
                    Data = { ["args"] = new { id, userId } }
                };
            if (app.Offers.Any(x => x.OrderId.HasValue))
                new { id, userId, offerId = app.Offers.FirstOrDefault(x => x.OrderId.HasValue).Id }.ThrowApplicationException("На это Обьявление уже сформирован заказ поэтому его нельзя удалить!");
            _db.Applications.Remove(app);
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить отклик вместе со всеми ее коментариями 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpDelete("seller/individual-applications/offers/{id}")]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> RemoveOffer(Guid id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var offer = await _db.ApplicationOffers
                .FirstOrDefaultAsync(x => x.Id == id && pids.Contains(x.ShopId));
            if (offer == null)
                throw new ApplicationException("Не удалость найти отклик у этого магазина!")
                {
                    Data = { ["args"] = new { id, pids } }
                };
            if (offer.OrderId.HasValue)
                new { offer.OrderId }.ThrowApplicationException("Нельзя удалить этот отклик потому что он уже добавлен в заказ");
            _db.ApplicationOffers.Remove(offer);
            return await _db.SaveChangesAsync();
        }
    }
}
