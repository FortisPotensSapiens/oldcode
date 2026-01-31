using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hike.BackgroundWorks
{
    public class CheckingPaymentStatusBackgroundWork : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<CheckingPaymentStatusBackgroundWork> _logger;

        public CheckingPaymentStatusBackgroundWork(IServiceProvider sp, ILogger<CheckingPaymentStatusBackgroundWork> logger)
        {
            _sp = sp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Выполняем задачу пока не будет запрошена остановка приложения
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var s = _sp.CreateScope())
                    {
                        var yk = s.ServiceProvider.GetRequiredService<IYooKassaClient>();
                        var db = s.ServiceProvider.GetRequiredService<HikeDbContext>();
                        var _webSockets = s.ServiceProvider.GetRequiredService<IWebSocketsClient>();
                        var _pushNotifications = s.ServiceProvider.GetRequiredService<IPushNotificationsClient>();
                        var _email = s.ServiceProvider.GetRequiredService<IEmailClient>();
                        var i = 0;
                        List<Entities.OrderDto> orders = await db
                            .Orders
                            .Where(x => x.State == Entities.OrderState.Created)
                            .Where(x => x.ExternalIdInPaymentSystem != null)
                            .Include(x => x.Items)
                            .Skip(i)
                            .Take(1000)
                            .ToListAsync();
                        i += 1000;
                        while (orders.Count > 0)
                        {
                            foreach (var order in orders)
                            {
                                if (string.IsNullOrWhiteSpace(order.ExternalIdInPaymentSystem))
                                    continue;
                                if (order.State != OrderState.Created)
                                    continue;
                                try
                                {
                                    var status = await yk.GetPaymentStatus(order.ExternalIdInPaymentSystem);
                                    if (status == "succeeded")
                                    {
                                        order.State = OrderState.Paid;
                                        order.PaymentDate = DateTime.UtcNow;
                                        if (order.Type == OrderType.Standard)
                                        {
                                            var oids = order.Items.Where(x => x.ItemId.HasValue).Select(x => x.ItemId.Value).ToList();
                                            var ms = await db.Merchandises.Where(x => oids.Contains(x.Id)).ToListAsync();
                                            foreach (var m in ms)
                                            {
                                                var item = order.Items.First(x => x.ItemId == m.Id);
                                                m.AvailableQuantity -= item.Amount;
                                            }
                                        }
                                        await db.SaveChangesAsync();
                                        var adminRole = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedName == "ADMIN");
                                        var admins = await db.UserRoles.AsNoTracking().Where(r => r.RoleId == adminRole.Id)
                                            .Select(x => x.UserId).ToListAsync();
                                        try
                                        {
                                            var users = await db.Users
                                                 .Where(x => x.Id == order.BuyerInfo.Id || x.Partners.Any(p => p.PartnerId == order.SellerInfo.Id) || admins.Contains(x.Id))
                                                 .AsNoTracking()
                                                 .ToListAsync();
                                            foreach (var user in users)
                                            {
                                                await _webSockets.OrderStatusChanged(user.Id, OrderState.Paid);
                                                var devices = await db.Devices.Where(x => x.UserId == user.Id).ToListAsync();
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
                                        var emails = await db.Users.Where(u => admins.Contains(u.Id))
                                            .Select(x => x.Email)
                                            .ToListAsync();
                                        emails.Add(order.SellerInfo.ContactEmail);
                                        foreach (var email in emails)
                                        {

                                            await _email.SendEmailAsync(email, $"Заказ номер {order.Number} оплачен!", sb.ToString());
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e, "Ошибка при проверке статуса платижа для заказа {OrderId}!", order.Id);
                                }
                            }
                            await db.SaveChangesAsync();
                            orders = await db
                                .Orders
                                .Where(x => x.State == Entities.OrderState.Created)
                                .Where(x => x.ExternalIdInPaymentSystem != null)
                                .Skip(i)
                                .Take(1000)
                                .ToListAsync();
                            i += 1000;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // обработка ошибки однократного неуспешного выполнения фоновой задачи
                    _logger.LogError(ex, "Ошибка при проверке статуса платежей!");
                }

                await Task.Delay(5000);
            }
        }
    }
}
