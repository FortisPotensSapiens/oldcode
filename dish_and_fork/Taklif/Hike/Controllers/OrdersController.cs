using System.Security.Claims;
using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Hike.Modules.AdminSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class OrdersController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly ICancellationTokensRepository _cancellationTokens;
        private readonly IDostavistaClient _dostavista;
        private readonly IAdminSettingsRepository _adminSettings;

        public OrdersController(
            HikeDbContext db,
            ICancellationTokensRepository cancellationTokens,
              IDostavistaClient dostavista,
            IAdminSettingsRepository adminSettings
            )
        {
            _db = db;
            _cancellationTokens = cancellationTokens;
            _dostavista = dostavista;
            _adminSettings = adminSettings;
        }

        /// <summary>
        /// Создать индивидуальный заказ с самовывозом
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("orders/individual-self-delivered")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateIndividualSelfDelivered(OrderIndividualSelfDeliveredCreateModel model)
        {
            var offer = await _db.ApplicationOffers.FirstOrDefaultAsync(x => x.Id == model.OfferId,
                _cancellationTokens.GetDefault());
            if (offer == null)
                throw new ApplicationException("Не найден ответ на заявку на индивидуальный заказ")
                {
                    Data = { ["model"] = model }
                };
            if (await _db.Orders.AnyAsync(x => x.Items.Any(i => i.OfferId == offer.Id)))
                new { model }.ThrowApplicationException("Вы уже принимали эту заявку");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new ApplicationException("Не найден профиль для пользователя") { Data = { ["userId"] = userId } };
            var seller = await _db.Shops.Include(x => x.Partner).FirstOrDefaultAsync(x => x.Id == offer.ShopId);
            if (seller == null)
                throw new ApplicationException("Не найден профиль для магазина") { Data = { ["sellerId"] = offer.ShopId } };
            if (!seller.Partner.IsPickupEnabled)
                new { model, shopId = seller.Id }.ThrowApplicationException("У магазина запрещен самовывоз!");
            var order = model.ToOrder(user.Id, offer);
            order.BuyerInfo = BuyerInfoDto.From(user);
            order.SellerInfo = SellerInfoDto.From(seller);
            order.Amount = offer.Sum;
            order.Type = OrderType.Individual;
            order.AdminSettings = await _adminSettings.Get();
            order.SetTotalPrice();
            _db.Orders.Add(order);
            offer.OrderId = order.Id;
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
            return order.Id;
        }

        /// <summary>
        /// Создать индивидуальный заказ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("orders/individual")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateIndividual(OrderIndividualCreateModel model)
        {
            var offer = await _db.ApplicationOffers.FirstOrDefaultAsync(x => x.Id == model.OfferId,
                _cancellationTokens.GetDefault());
            if (offer == null)
                throw new ApplicationException("Не найден ответ на заявку на индивидуальный заказ")
                {
                    Data = { ["model"] = model }
                };
            if (await _db.Orders.AnyAsync(x => x.Items.Any(i => i.OfferId == offer.Id)))
                new { model }.ThrowApplicationException("Вы уже принимали эту заявку");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new ApplicationException("Не найден профиль для пользователя") { Data = { ["userId"] = userId } };
            var seller = await _db.Shops.Include(x => x.Partner).FirstOrDefaultAsync(x => x.Id == offer.ShopId);
            if (seller == null)
                throw new ApplicationException("Не найден профиль для магазина") { Data = { ["sellerId"] = offer.ShopId } };
            var order = model.ToOrder(user.Id, offer);
            order.BuyerInfo = BuyerInfoDto.From(user);
            order.SellerInfo = SellerInfoDto.From(seller);
            order.Amount = offer.Sum;
            order.Type = OrderType.Individual;
            order.DeliveryInfo = await GetDeliveryPrice(order);
            order.AdminSettings = await _adminSettings.Get();
            order.SetTotalPrice();
            _db.Orders.Add(order);
            offer.OrderId = order.Id;
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
            return order.Id;
        }

        /// <summary>
        /// Создать заказ с самовывозом
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("orders/self-delivered")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateSelfDelivered(OrderSerfDeliveredCreateModel model)
        {
            if (model.Items == null || model.Items.Count < 1)
                throw new ApplicationException("В заказе должен быть хотябы один товар");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new ApplicationException("Не найден профиль для пользователя") { Data = { ["userId"] = userId } };
            var itemIds = model.Items.Select(i => i.ItemId);
            var goods = await _db.Merchandises.AsNoTracking().Where(x => itemIds.Contains(x.Id)).Include(x => x.Shop).ThenInclude(x => x.Partner).ToListAsync();
            var seller = goods.First().Shop;
            if (seller == null)
                throw new ApplicationException("Не найден профиль для магазина") { Data = { ["goods"] = model.Items } };
            if (!seller.Partner.IsPickupEnabled)
                new { model, shopId = seller.Id }.ThrowApplicationException("У магазина запрещен самовывоз!");
            var order = model.ToOrder(user.Id, goods);
            order.BuyerInfo = BuyerInfoDto.From(user);
            order.SellerInfo = SellerInfoDto.From(seller);
            var sum = goods.OrderBy(g => g.Id).Zip(order.Items.OrderBy(i => i.ItemId), (a, b) => a.Price?.Value * b.Amount).Sum();
            order.Amount = sum.HasValue ? new MoneyDto { CurrencyType = CurrencyType.Rub, Value = sum.Value } : null;
            order.DeliveryInfo = await GetDeliveryPrice(order);
            order.AdminSettings = await _adminSettings.Get();
            order.SetTotalPrice();
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order.Id;
        }


        /// <summary>
        /// Создать базовый заказ
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("orders")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(OrderCreateModel model)
        {
            if (model.Items == null || model.Items.Count < 1)
                throw new ApplicationException("В заказе должен быть хотябы один товар");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new ApplicationException("Не найден профиль для пользователя") { Data = { ["userId"] = userId } };
            var itemIds = model.Items.Select(i => i.ItemId);
            var goods = await _db.Merchandises.AsNoTracking().Where(x => itemIds.Contains(x.Id)).Include(x => x.Shop).ThenInclude(x => x.Partner).ToListAsync();
            var seller = goods.First().Shop;
            if (seller == null)
                throw new ApplicationException("Не найден профиль для магазина") { Data = { ["goods"] = model.Items } };
            var order = model.ToOrder(user.Id, goods);
            order.BuyerInfo = BuyerInfoDto.From(user);
            order.SellerInfo = SellerInfoDto.From(seller);
            var sum = goods.OrderBy(g => g.Id).Zip(order.Items.OrderBy(i => i.ItemId), (a, b) => a.Price.Value * b.Amount).Sum();
            order.Amount = new MoneyDto { CurrencyType = CurrencyType.Rub, Value = sum };
            order.DeliveryInfo = await GetDeliveryPrice(order);
            order.AdminSettings = await _adminSettings.Get();
            order.SetTotalPrice();
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order.Id;
        }

        /// <summary>
        /// Получить заказ текущего авторизованного пользователя по идентификатору заказа
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("orders/{id}")]
        [ProducesResponseType(200, Type = typeof(OrderReadModel))]
        public async Task<OrderReadModel> Get(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = await _db.Orders.AsNoTracking()
                // .Include(x => x.Buyer)
                // .ThenInclude(x => x.User)
                .Include(x => x.Items)
                // .ThenInclude(x => x.Item)
                // .ThenInclude(x => x.Partner)
                .FirstOrDefaultAsync(x => x.Id == id && x.BuyerInfo.Id == userId);
            if (dto == null)
                throw new ApplicationException("Не удалось найти заказ")
                {
                    Data = { ["id"] = id, ["userId"] = userId }
                };
            return OrderReadModel.From(dto, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        }

        /// <summary>
        /// Получить заказ сделанный текущему продавцу
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("seller/orders/{id}")]
        [ProducesResponseType(200, Type = typeof(OrderReadModel))]
        public async Task<OrderReadModel> GetForSeller(Guid id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var dto = await GetFilteredBySeller(pids)
                // .Include(x => x.Buyer)
                // .ThenInclude(x => x.User)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                throw new ApplicationException("Не удалось найти заказ")
                {
                    Data = { ["id"] = id, ["userId"] = sellerId }
                };
            return OrderReadModel.From(dto, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
        }

        /// <summary>
        /// Получить список заказов текущего пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("orders/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<OrderReadModel>))]
        public async Task<PageResultModel<OrderReadModel>> Filter(PaginationModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dtos = await GetFiltered(userId)
                .Include(x => x.Items)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await GetFiltered(userId).CountAsync();
            return new PageResultModel<OrderReadModel>()
            {
                TotalCount = count,
                Items = dtos.Select(x => OrderReadModel.From(x, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}")).ToList()
            };
        }

        /// <summary>
        /// Получить список заказов поступивших текущему продавцу
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPost("seller/orders/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<OrderReadModel>))]
        public async Task<PageResultModel<OrderReadModel>> FilterSeller(PaginationModel model)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var dtos = await GetFilteredBySeller(pids)
                .Include(x => x.Items)
                //  .ThenInclude(x => x.Item)
                //  .ThenInclude(x => x.Partner)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await GetFilteredBySeller(pids).CountAsync();
            return new PageResultModel<OrderReadModel>()
            {
                TotalCount = count,
                Items = dtos.Select(x => OrderReadModel.From(x, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}")).ToList()
            };
        }

        private async Task<DeliveryOrderResponseDto?> GetDeliveryPrice(OrderDto order)
        {
            if (order.DeliveryType != OrderDeliveryType.SelfDelivered)
            {
                var offerDeliveryDate = order.Items.First().OfferDeliveryDate;
                if (!offerDeliveryDate.HasValue)
                    offerDeliveryDate = DateTime.UtcNow.AddHours(4);
                var intervals = await _dostavista.GetDeliveryIntervals(offerDeliveryDate.Value.Date >= DateTime.UtcNow.Date ? offerDeliveryDate.Value : DateTime.UtcNow.AddHours(4));
                var interval = intervals.GetEarliest();
                var request = new DeliveryOrderModel { OrderId = order.Id }.ToDostavistaCalculateOrderRequest(order, interval);
                var response = await _dostavista.CalculateOrder(request);
                if (!response.IsSuccessful)
                    throw new ApplicationException("Ошибка при попытке вычислить стоимость доставки заказа!")
                    {
                        Data = { ["args"] = new { response } }
                    };
                return DeliveryOrderResponseDto.ToDeliveryOrderResponseModel(response);
            }
            return null;
        }

        private IQueryable<OrderDto> GetFilteredBySeller(List<Guid> pids) =>
            _db.Orders
             .FromSqlRaw($"SELECT * FROM  public.\"{nameof(_db.Orders)}\" o WHERE o.\"{nameof(OrderDto.SellerInfo)}\"->>'Id'" + " = ANY({0}::text[])", pids)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .Where(x => x.State != OrderState.Created);

        private IQueryable<OrderDto> GetFiltered(string userId) =>
            _db.Orders.AsNoTracking().Where(x => x.BuyerInfo.Id == userId)
                .OrderByDescending(x => x.Created);
    }
}
