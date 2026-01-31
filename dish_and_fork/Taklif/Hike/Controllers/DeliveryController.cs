using Hike.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hike.Clients;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Hike.Entities;
using Hike.Attributes;
using Hike.Extensions;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Hike.Ef;
using Hike.Modules.AdminSettings;

namespace Hike.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDostavistaClient _dostavista;
        private readonly HikeDbContext _db;
        private readonly ICancellationTokensRepository _cancellationTokens;
        private readonly ILogger<DeliveryController> _logger;
        private readonly IWebSocketsClient _webSockets;
        private readonly IConfiguration _configuration;
        private readonly IPushNotificationsClient _pushNotifications;
        private readonly IAdminSettingsRepository _adminSettings;

        public DeliveryController(
            IDostavistaClient dostavista,
            HikeDbContext db,
            ICancellationTokensRepository cancellationTokens,
            ILogger<DeliveryController> logger,
            IWebSocketsClient webSockets,
            IConfiguration configuration,
            IPushNotificationsClient pushNotifications,
            IAdminSettingsRepository adminSettings
            )
        {
            _dostavista = dostavista;
            _db = db;
            _cancellationTokens = cancellationTokens;
            _logger = logger;
            _webSockets = webSockets;
            _configuration = configuration;
            _pushNotifications = pushNotifications;
            _adminSettings = adminSettings;
        }

        /// <summary>
        /// Получить список доступных интервалов на дату доставки. Если не указать дату то выдаст ближайщие доступные
        /// </summary>
        /// <param name="date">Дата доставки (день.месяц.год)</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("delivery/available-delivery-intervals")]
        [ProducesResponseType(200, Type = typeof(List<DeliveryInterval>))]
        public async Task<List<DeliveryInterval>> GetDeliveryIntervals(DateTime? date = null)
        {
            var response = await _dostavista.GetDeliveryIntervals(date);
            return response.DeliveryIntervals;
        }

        /// <summary>
        /// Вычисляет стоимость доставки отклика обычного заказа
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [HttpPost("delivery/standart-order/price")]
        [ProducesResponseType(200, Type = typeof(CalculateDeliveryPriceReadModel))]
        public async Task<CalculateDeliveryPriceReadModel> DeliveryNowPrice(DeliveryNowPriceModel model)
        {
            var ids = model.Items.Select(x => x.ItemId).ToList();
            var items = await _db.Merchandises
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .ToListAsync(_cancellationTokens.GetDefault());
            if (string.IsNullOrWhiteSpace(items?.FirstOrDefault()?.Shop?.Partner?.Address?.House))
                new { }.ThrowApplicationException("У продавца не указан адрес откуда забирать товар. Обратитесь в служну поддержчи чтобы они попросили продавца указать свой адрес");
            var intervals = await _dostavista.GetDeliveryIntervals();
            var interval = intervals.GetEarliest();
            var request = model.ToDostavistaCalculateOrderRequest(items, interval);
            var response = await _dostavista.CalculateOrder(request);
            if (!response.IsSuccessful)
                throw new ApplicationException("Ошибка при попытке вычислить стоимость заказа!")
                {
                    Data = { ["args"] = new
                    {
                        response
                    }}
                };
            var point = response.Order.points.OrderByDescending(x => x.required_start_datetime).ThenByDescending(x => x.required_finish_datetime).First();
            var price = decimal.Parse(response.Order.payment_amount);
            var adminSettings = await _adminSettings.Get();
            price = adminSettings.CalculateDeliveryTotalPrice(price);
            return new CalculateDeliveryPriceReadModel { Price = price, StartTime = point.required_start_datetime, FinishTime = point.required_finish_datetime };
        }

        /// <summary>
        /// Вычисляет стоимость доставки отклика (Offer)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [HttpPost("delivery/individual-order/price")]
        [ProducesResponseType(200, Type = typeof(CalculateDeliveryPriceReadModel))]
        public async Task<CalculateDeliveryPriceReadModel> DeliveryIndividualPrice(DeliveryIndividualPriceModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var offer = await _db.ApplicationOffers
                .AsNoTracking()
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .FirstOrDefaultAsync(x => x.Id == model.OfferId && x.Application.Customer.Id == userId, _cancellationTokens.GetDefault());
            if (string.IsNullOrWhiteSpace(offer?.Shop?.Partner?.Address?.House))
                new { }.ThrowApplicationException("У продавца не указан адрес откуда забирать товар. Обратитесь в служну поддержчи чтобы они попросили продавца указать свой адрес");
            var intervals = await _dostavista.GetDeliveryIntervals();
            var interval = intervals.GetEarliest();
            var request = model.ToDostavistaCalculateOrderRequest(offer, interval);
            var response = await _dostavista.CalculateOrder(request);
            if (!response.IsSuccessful)
                throw new ApplicationException("Ошибка при попытке вычислить стоимость заказа!")
                {
                    Data = { ["args"] = new
                    {
                        response
                    }}
                };
            var point = response.Order.points.OrderByDescending(x => x.required_start_datetime).ThenByDescending(x => x.required_finish_datetime).First();
            var price = decimal.Parse(response.Order.payment_amount);
            var adminSettings = await _adminSettings.Get();
            price = adminSettings.CalculateDeliveryTotalPrice(price);
            return new CalculateDeliveryPriceReadModel { Price = price, StartTime = point.required_start_datetime, FinishTime = point.required_finish_datetime };
        }

        /// <summary>
        /// Отдает службе доставке команду на доставку данного заказа
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPost("seller/delivery/order")]
        [ProducesResponseType(200, Type = typeof(DeliveryOrderResponseModel))]
        public async Task<DeliveryOrderResponseModel> DeliveryOrder(DeliveryOrderModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _db.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == model.OrderId);
            if (!await _db.Partners.AnyAsync(x => x.Id == order.SellerInfo.Id && x.Employes.Any(e => e.UserId == userId)))
                new { model, userId, order }.ThrowApplicationException("Вы не можете отправить в службу доставки данный заказ потому что он сделан для другого продавца!");
            if (order.State != Entities.OrderState.Paid)
                throw new ApplicationException("Заказ еще не оплачен!");
            //var offerDeliveryDate = order.Items.First().OfferDeliveryDate;
            //if (!offerDeliveryDate.HasValue)
            //    offerDeliveryDate = DateTime.UtcNow;
            var intervals = await _dostavista.GetDeliveryIntervals(); //.GetDeliveryIntervals(offerDeliveryDate.Value.Date >= DateTime.UtcNow.AddHours(4).Date ? offerDeliveryDate.Value : DateTime.UtcNow.AddHours(4));
            var interval = intervals.GetEarliest();
            var request = model.ToDostavistaCalculateOrderRequest(order, interval);
            var response = await _dostavista.CreateOrder(request);
            if (!response.IsSuccessful)
                throw new ApplicationException("Ошибка при попытке запустить доставку заказа!")
                {
                    Data = { ["args"] = new
                    {
                        response
                    }}
                };
            order.DeliveryInfo = DeliveryOrderResponseDto.ToDeliveryOrderResponseModel(response);
            order.State = OrderState.Delivering;
            order.ExternalIdInDeliverySystem = response.Order.order_id.ToString();
            await _db.SaveChangesAsync();
            return DeliveryOrderResponseModel.ToDeliveryOrderResponseModel(response);
        }

        [HttpPost("dostavista/callback")]
        public async Task DeliveryStatusCallback()
        {
            using var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8);
            var txt = reader.ReadToEnd();
            var signature = Request.Headers["HTTP_X_DV_SIGNATURE"].ToString();
            _logger.LogInformation("DOSTAVICTA CALLBACK DATA!");
            _logger.LogInformation(signature);
            _logger.LogInformation(txt);
            using var sha256 = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(_configuration["Dostavista:CallBackToken"]));
            var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(txt));
            if (!System.Text.Encoding.UTF8.GetBytes(signature).SequenceEqual(hash))
                new { txt, signature, hash }.ThrowApplicationException("Подпись не соответствует подписиси в запросе!");
            var data = txt.ToObject<dynamic>();
            var sellerUrl = data.order.points[0].tracking_url?.ToString();
            var buyerUrl = data.order.points[1].tracking_url?.ToString();
            if (data.order?.status?.ToString() != "completed")
                return;
            foreach (var p in data.order.points)
            {
                string id = p.client_order_id?.ToString();
                if (string.IsNullOrWhiteSpace(id))
                    continue;
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                order.State = Entities.OrderState.Delivered;
                if (!string.IsNullOrWhiteSpace(sellerUrl))
                    order.DeliveryInfo.SellerDeliveryTrackingUrl = sellerUrl;
                if (!string.IsNullOrWhiteSpace(buyerUrl))
                    order.DeliveryInfo.BuyerDeliveryTrackingUrl = buyerUrl;
                await _db.SaveChangesAsync();
                try
                {
                    var users = await _db.Users
                         .Where(x => x.Id == order.BuyerId || x.Partners.Any(p => p.PartnerId == order.SellerInfo.Id))
                         .AsNoTracking()
                         .ToListAsync();
                    foreach (var user in users)
                    {
                        await _webSockets.OrderStatusChanged(user.Id, OrderState.Delivered);
                        await _pushNotifications.SendAsync(user.Id, new FcmRequest { Message = new FcmMessage { Notification = new FcmNotification { Title = $"Ваш заказ номер {order.Number} Доставлен!" } } });
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка при отправке уведомлений");
                }
            }
        }
    }
}


//required_start_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (от).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_start_datetime не должен быть задан, а на второй точке required_start_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов

//required_finish_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (до).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_finish_datetime не должен быть задан, а на второй точке required_finish_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов
