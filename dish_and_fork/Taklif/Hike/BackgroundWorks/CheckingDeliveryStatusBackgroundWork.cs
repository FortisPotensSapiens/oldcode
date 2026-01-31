using System.Threading;
using System.Threading.Tasks;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hike.BackgroundWorks
{
    public class CheckingDeliveryStatusBackgroundWork : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger<CheckingDeliveryStatusBackgroundWork> _logger;

        public CheckingDeliveryStatusBackgroundWork(IServiceProvider sp, ILogger<CheckingDeliveryStatusBackgroundWork> logger)
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
                        var dk = s.ServiceProvider.GetRequiredService<IDostavistaClient>();
                        var db = s.ServiceProvider.GetRequiredService<HikeDbContext>();
                        var i = 0;
                        List<Entities.OrderDto> orders = await db
                            .Orders
                            .Where(x => x.State == Entities.OrderState.Delivering)
                            .Where(x => x.ExternalIdInDeliverySystem != null)
                            .Skip(i)
                            .Take(1000)
                            .ToListAsync();
                        i += 1000;
                        while (orders.Count > 0)
                        {
                            foreach (var item in orders)
                            {
                                if (string.IsNullOrWhiteSpace(item.ExternalIdInDeliverySystem))
                                    continue;
                                try
                                {
                                    var dord = await dk.GetOrder(uint.Parse(item.ExternalIdInDeliverySystem));
                                    if (dord.status == DostavistaOrderStatus.Completed)
                                    {
                                        item.State = OrderState.Delivered;
                                        item.DeliveredDate = DateTime.UtcNow;
                                    }
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e, "Ошибка при проверке статуса доставки для заказа {OrderId}!", item.Id);
                                }
                            }
                            await db.SaveChangesAsync();
                            orders = await db
                                .Orders
                                .Where(x => x.State == Entities.OrderState.Delivering)
                                .Where(x => x.ExternalIdInDeliverySystem != null)
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
                    _logger.LogError(ex, "Ошибка при проверке статуса доставки!");
                }

                await Task.Delay(5000);
            }
        }
    }
}
