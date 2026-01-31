using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Daf.SharedModule.Domain.BaseVo;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Modules.AdminSettings;
using Hike.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hike.Controllers
{
    [Route("api/v1/pay-any-way")]
    [ApiController]
    public partial class PaymentsController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailClient _email;
        private readonly IWebSocketsClient _webSockets;
        private readonly IDostavistaClient _dostavista;
        private readonly IPushNotificationsClient _pushNotifications;
        private readonly IYooKassaClient _yooKassa;
        private readonly IBaseUriRepository _baseUri;
        private readonly IAdminSettingsRepository _adminSettings;

        public PaymentsController(
            HikeDbContext db,
            ILogger<PaymentsController> logger,
            IConfiguration configuration,
            IEmailClient email,
            IWebSocketsClient webSockets,
            IDostavistaClient dostavista,
            IPushNotificationsClient pushNotifications,
            IYooKassaClient yooKassa,
            IBaseUriRepository baseUri,
            IAdminSettingsRepository adminSettings
            )
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
            _email = email;
            _webSockets = webSockets;
            _dostavista = dostavista;
            _pushNotifications = pushNotifications;
            _yooKassa = yooKassa;
            _baseUri = baseUri;
            _adminSettings = adminSettings;
        }

        /// <summary>
        /// Оплатить заказ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("pay-order/{id}")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<string> PayOrder(Guid id)
        {
            /// return "api/v1/pay-any-way/default";
            var order = await _db.Orders
                .Include(x => x.Items)
                // .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                throw new ApplicationException("Не найден заказ")
                {
                    Data = { ["id"] = id }
                };
            if (order.State != OrderState.Created)
                throw new ApplicationException("Этот заказ нельзя оплатить")
                {
                    Data = { ["id"] = id }
                };
            var partner = await _db.Partners.AsNoTracking().FirstOrDefaultAsync(x => x.Id == order.SellerInfo.Id);
            var dp = order.DeliveryInfo?.DeliveryPrice?.Value ?? 0;
            var ip = order.GetTotalAmount();
            var adminSettings = await _adminSettings.Get();
            if (order.DeliveryType != OrderDeliveryType.SelfDelivered)
                dp = adminSettings.CalculateDeliveryTotalPrice(dp);
            ip = adminSettings.CalculateOrderTotalPrice(ip);
            var data = await _yooKassa.CratePayment(order.Id.ToString(), dp, ip,
             new Uri(_baseUri.Get(), $"api/v1/pay-any-way/default/{order.Id}").AbsoluteUri,
                order.Id.ToString(), partner.ExternalId);
            order.ExternalIdInPaymentSystem = data.id;
            await _db.SaveChangesAsync();
            return data.redirect;
        }

        [HttpGet("default/{id}")]
        public async Task<IActionResult> Default(Guid id)
        {
            var order = await _db.Orders
                .Include(i => i.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
            var status = await _yooKassa.GetPaymentStatus(order.ExternalIdInPaymentSystem);
            if (status == "succeeded")
            {
                if (order.State != OrderState.Created)
                    return Redirect("~/orders");
                order.State = OrderState.Paid;
                order.PaymentDate = DateTime.UtcNow;
                if (order.Type == OrderType.Standard)
                {
                    var oids = order.Items.Where(x => x.ItemId.HasValue).Select(x => x.ItemId.Value).ToList();
                    var ms = await _db.Merchandises.Where(x => oids.Contains(x.Id)).ToListAsync();
                    foreach (var m in ms)
                    {
                        var item = order.Items.First(x => x.ItemId == m.Id);
                        m.AvailableQuantity -= item.Amount;
                    }
                }
                await _db.SaveChangesAsync();
                var adminRole = await _db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == "ADMIN");
                var admins = await _db.UserRoles.AsNoTracking().Where(r => r.RoleId == adminRole.Id)
                    .Select(x => x.UserId).ToListAsync();
                try
                {
                    var users = await _db.Users
                         .Where(x => x.Id == order.BuyerInfo.Id || x.Partners.Any(p => p.PartnerId == order.SellerInfo.Id) || admins.Contains(x.Id))
                         .AsNoTracking()
                         .ToListAsync();
                    foreach (var user in users)
                    {
                        await _webSockets.OrderStatusChanged(user.Id, OrderState.Paid);
                        var devices = await _db.Devices.Where(x => x.UserId == user.Id).ToListAsync();
                        foreach (var device in devices)
                        {
                            await _pushNotifications.SendAsync(device.FcmPushToken, new FcmRequest { Message = new FcmMessage { Notification = new FcmNotification { Title = $"Ваш заказ номер {order.Number} оплачен!" } } });
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Ошибка при отправке уведомлений");
                }
                var sb = new StringBuilder();
                sb.Append("<div>");
                sb.AppendLine($"<H1>Заказ номер {order.Number} оплачен!</H1>");
                if (order.Type == OrderType.Standard && order.Items.Count > 0)
                {
                    foreach (var item in order.Items)
                    {
                        sb.AppendLine($"<p>Товар: {item.Title}  Количество: {item.Amount}</p>");
                    }
                }
                sb.AppendLine($"<p>Доставка: {(order.DeliveryType == OrderDeliveryType.SelfDelivered ? "Самовывоз" : "Курьер")}</p>");
                sb.Append("</div>");
                var emails = await _db.Users.Where(u => admins.Contains(u.Id))
                    .Select(x => x.Email)
                    .ToListAsync();
                emails.Add(order.SellerInfo.ContactEmail);
                foreach (var email in emails)
                {
                    await _email.SendEmailAsync(email, $"Заказ номер {order.Number} оплачен!", sb.ToString());
                }
                return Redirect("~/orders");
            }
            return Redirect("~/orders");
        }

        /// <summary>
        /// Метод обратного вызова
        /// </summary>
        /// <returns></returns>
        [HttpGet("callback")]
        [ProducesResponseType(200, Type = typeof(string))]
        public async Task<string> Callback([FromQuery] PayAnyWayCallbackModel model)
        {
            _logger.LogWarning("Paytodo Callback data {Query}", Request.QueryString.Value);
            if (_configuration["IsTesting"]?.ToLower() != "true" && !model.IsValid())
                throw new ApplicationException("Не валидный запросс") { Data = { ["data"] = model } };
            var id = Guid.Parse(model.MNT_TRANSACTION_ID);
            var order = await _db.Orders
                .Include(x => x.Items)
                //  .ThenInclude(x => x.Item)
                //  .ThenInclude(x => x.Partner)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
                return "FAIL";
            if (order.State != OrderState.Created)
                return "FAIL";
            order.State = OrderState.Paid;
            order.PaymentDate = DateTime.UtcNow;
            if (order.Type == OrderType.Standard)
            {
                var oids = order.Items.Where(x => x.ItemId.HasValue).Select(x => x.ItemId.Value).ToList();
                var ms = await _db.Merchandises.Where(x => oids.Contains(x.Id)).ToListAsync();
                foreach (var m in ms)
                {
                    var item = order.Items.First(x => x.ItemId == m.Id);
                    m.AvailableQuantity -= item.Amount;
                }
            }
            await _db.SaveChangesAsync();
            try
            {
                var users = await _db.Users
                     .Where(x => x.Id == order.BuyerInfo.Id || x.Partners.Any(p => p.PartnerId == order.SellerInfo.Id))
                     .AsNoTracking()
                     .ToListAsync();
                foreach (var user in users)
                {
                    await _webSockets.OrderStatusChanged(user.Id, OrderState.Paid);
                    var devices = await _db.Devices.Where(x => x.UserId == user.Id).ToListAsync();
                    foreach (var device in devices)
                    {
                        await _pushNotifications.SendAsync(device.FcmPushToken, new FcmRequest { Message = new FcmMessage { Notification = new FcmNotification { Title = $"Ваш заказ номер {order.Number} оплачен!" } } });
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при отправке уведомлений");
            }
            var sb = new StringBuilder();
            sb.Append("<div>");
            sb.AppendLine($"<H1>Заказ номер {order.Number} оплачен!</H1>");
            if (order.Type == OrderType.Standard && order.Items.Count > 0)
            {
                foreach (var item in order.Items)
                {
                    sb.AppendLine($"<p>Товар: {item.Title}  Количество: {item.Amount}</p>");
                }
            }
            sb.AppendLine($"<p>Доставка: {(order.DeliveryType == OrderDeliveryType.SelfDelivered ? "Самовывоз" : "Курьер")}</p>");
            sb.Append("</div>");
            var emails = new[] { order.SellerInfo.ContactEmail };
            foreach (var email in emails)
            {

                await _email.SendEmailAsync(email, $"Заказ номер {order.Number} оплачен!", sb.ToString());
            }
            return "SUCCESS";
        }
    }
}
