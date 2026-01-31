using System.Text.RegularExpressions;
using System.Threading;
using AleBotApi.Clients;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.EventsAndSagas;
using AleBotApi.Services;
using AleBotApi.Services.ServiceBindings.EmailService;
using ApeBotApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace AleBotApi.BackgroundWorks
{
    public class EventsAndSagasHandling : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<EventsAndSagasHandling> _logger;
        private IDarkbandRuClient _darkband;
        private IEmailService _emailService;
        private AbDbContext _db;

        public EventsAndSagasHandling(
            IServiceProvider services,
            ILogger<EventsAndSagasHandling> logger
            )
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var sp = scope.ServiceProvider;

                    var db = sp.GetRequiredService<AbDbContext>();
                    _darkband = sp.GetRequiredService<IDarkbandRuClient>();
                    _emailService = sp.GetRequiredService<IEmailService>();
                    _db = db;
                    List<AbEventOrSagaDbDto> unhandledEvents;
                    unhandledEvents = await db.EventsAndSagas.Where(x => x.CompletedDate == null).ToListAsync();
                    foreach (var item in unhandledEvents)
                    {
                        await HandleEvent(item, db, stoppingToken);
                    }
                    if (unhandledEvents.Count > 0)
                        await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on globan run");
                }

                await Task.Delay(3000, stoppingToken);
            }
        }

        private async Task HandleEvent(AbEventOrSagaDbDto eventOrSagaDbDto, AbDbContext db, CancellationToken stoppingToken)
        {
            var data = eventOrSagaDbDto.Body?.ToObject<OrderPaid>();
            try
            {
                if (eventOrSagaDbDto.Type == DbDtos.Enums.AbEventOrSagaType.OrderPaid)
                {
                    await HandleOrderPaid(eventOrSagaDbDto, db, data, stoppingToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on HandleEvent");
            }
            eventOrSagaDbDto.Body = data?.ToJson();
        }

        private async Task HandleOrderPaid(AbEventOrSagaDbDto eventOrSagaDbDto, AbDbContext db, OrderPaid? orderPaid, CancellationToken stoppingToken)
        {
            if (orderPaid == null)
                new { }.ThrowApplicationException("No event data");
            var orderLines = await db.OrderLines.Where(x => x.OrderId == orderPaid.OrderId).ToListAsync(cancellationToken: stoppingToken);
            var user = await db.Users.FindAsync(orderPaid.UserId)
               ?? throw new ArgumentNullException("Пользоваетль не найден");
            var order = await db.Orders.FindAsync(orderPaid.OrderId)
               ?? throw new ArgumentNullException("Заказ не найден");
            var merchIds = orderLines.Select(x => x.MerchId).ToList();
            var merchs = await _db.Merches
                  .Include(x => x.MerchServers)
                  .ThenInclude(x => x.Server)
                  .Include(x => x.MerchLicenses)
                  .Include(x => x.MerchServerExtentions)
                  .ThenInclude(x => x.ServerExtention)
                  .Where(x => merchIds.Contains(x.Id))
                  .ToListAsync();

            var hasErrors = false;
            if (!orderPaid.IsSendOrderPurchasedNotifyToAdmin && orderPaid.SendOrderPurchasedNotifyToAdminRetryCount < 30)
            {
                try
                {
                    orderPaid.SendOrderPurchasedNotifyToAdminRetryCount += 1;
                    await _emailService.SendOrderPurchasedNotifyToAdmin(new SendOrderPurchasedNotifyToAdminBinding
                    {
                        UserId = user.Id,
                        UserEmail = user.Email,
                        OrderId = order.Id,
                        OrderAmount = order.Amount,
                        OrderLines = orderLines
                      .Select(x => new SendOrderPurchasedNotifyToAdminBinding_OrderLine { Name = x.Name, Price = x.Price })
                      .ToList()
                    });
                    orderPaid.IsSendOrderPurchasedNotifyToAdmin = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "SendOrderPurchasedNotifyToAdmin");
                    hasErrors = true;
                }
            }
            if (!orderPaid.IsSendOrderPurchasedNotifyUser && orderPaid.SendOrderPurchasedNotifyUserCount < 30)
            {
                try
                {
                    orderPaid.SendOrderPurchasedNotifyUserCount += 1;
                    await _emailService.SendOrderPurchasedNotifyUser(order.Amount, orderLines.FirstOrDefault()?.Name, user.Email);
                    orderPaid.IsSendOrderPurchasedNotifyUser = true;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "SendOrderPurchasedNotifyUser");
                    hasErrors = true;
                }
            }

            foreach (var orderLine in orderLines)
            {
                var merch = merchs.FirstOrDefault(m => m.Id == orderLine.MerchId);
                if (merch == null)
                    continue;

                if (merch.MerchServers.Any() && merch.MerchLicenses.Any())
                {
                    if (!orderPaid.HandledMerchesIds.Contains(merch.Id))
                    {
                        try
                        {
                            var r = await _darkband.BuyLicenseWithServer(ToTariff(merch), order.UserId);
                            DateTime.TryParse(r.vps?.expiration, out var expirationDate);
                            expirationDate = new DateTime(expirationDate.Year, expirationDate.Month, expirationDate.Day, expirationDate.Hour, expirationDate.Minute, expirationDate.Second, DateTimeKind.Utc);
                            foreach (var s in merch.MerchServers)
                                _db.UserServers.Add(new AbUserServerDbDto
                                {
                                    Id = Guid.NewGuid(),
                                    ServerId = s.ServerId,
                                    UserId = order.UserId,
                                    Login = r.vps?.user,
                                    Password = r.vps?.password,
                                    Address = r.vps?.server,
                                    ExpirationDate = expirationDate == default ? DateTime.UtcNow.AddMonths((int)s.Server.ServerDurationInMonth) : expirationDate,
                                    OrderLineId = orderLine.Id,
                                    ExternalId = r.vps?.vps_id?.ToString()
                                });
                            foreach (var l in r.bots)
                            {
                                var name = l.product.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
                                _db.UserLicenses.Add(new AbUserLicenseDbDto
                                {
                                    Id = Guid.NewGuid(),
                                    LicenseId = _db.Licenses.First(x => x.Name == name).Id,
                                    UserId = order.UserId,
                                    ActivationKey = l.id.ToString(),
                                    OrderLineId = orderLine.Id
                                });
                            }
                            orderPaid.HandledMerchesIds.Add(merch.Id);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "BuyLicenseWithServer");
                            hasErrors = true;
                        }
                    }
                }
                else if (merch.MerchServers.Any())
                {
                    foreach (var s in merch.MerchServers)
                    {
                        if (!orderPaid.HandledMerchServerIds.Contains(s.Id))
                        {
                            try
                            {
                                var r = await _darkband.BuyOnlyVpsServer(order.UserId, ToServerTariff(s.Server.ServerDurationInMonth));
                                DateTime.TryParse(r?.expiration, out var expirationDate);
                                expirationDate = new DateTime(expirationDate.Year, expirationDate.Month, expirationDate.Day, expirationDate.Hour, expirationDate.Minute, expirationDate.Second, DateTimeKind.Utc);
                                _db.UserServers.Add(new AbUserServerDbDto
                                {
                                    Id = Guid.NewGuid(),
                                    ServerId = s.ServerId,
                                    UserId = order.UserId,
                                    Login = r?.user,
                                    Password = r?.password,
                                    Address = r?.server,
                                    ExpirationDate = expirationDate == default ? DateTime.UtcNow.AddMonths((int)s.Server.ServerDurationInMonth) : expirationDate,
                                    OrderLineId = orderLine.Id,
                                    ExternalId = r?.vps_id?.ToString()
                                });
                                orderPaid.HandledMerchServerIds.Add(s.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "BuyOnlyVpsServer");
                                hasErrors = true;
                            }
                        }
                    }
                }

                if (orderLine.ExtendedUserServerId.HasValue)
                {
                    foreach (var s in merch.MerchServerExtentions)
                    {
                        var us = await _db.UserServers.FirstOrDefaultAsync(x => x.Id == orderLine.ExtendedUserServerId.Value);
                        if (us == null)
                            continue;
                        if (!int.TryParse(us.ExternalId, out var vpsId))
                            continue;
                        if (!orderPaid.HandledMerchServerExtentionIds.Contains(s.Id))
                        {
                            try
                            {
                                var r = await _darkband.ExtendVps(vpsId, ToServerTariff(s.ServerExtention.ServerDurationInMonth));
                                DateTime.TryParse(r.vps.expiration, out var expirationDate);
                                expirationDate = new DateTime(expirationDate.Year, expirationDate.Month, expirationDate.Day, expirationDate.Hour, expirationDate.Minute, expirationDate.Second, DateTimeKind.Utc);
                                us.ExpirationDate = expirationDate == default ? DateTime.UtcNow.AddMonths((int)s.ServerExtention.ServerDurationInMonth) : expirationDate;
                                orderPaid.HandledMerchServerExtentionIds.Add(s.Id);
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "BuyOnlyVpsServer");
                                hasErrors = true;
                            }
                        }
                    }
                }
            }

            if (!hasErrors)
                eventOrSagaDbDto.CompletedDate = DateTime.UtcNow;
        }

        private DbrLicenseTarifs ToTariff(AbMerchDbDto merh)
        {
            if (merh.Name?.Trim() == "Ale Bot: Trial")
                return DbrLicenseTarifs.Trial;
            if (merh.Name?.Trim() == "Ale Bot: Invest")
                return DbrLicenseTarifs.Invest;
            if (merh.Name?.Trim() == "Ale Bot: Worker")
                return DbrLicenseTarifs.Worker;
            return DbrLicenseTarifs.VIP;
        }

        private DbrVpsServersTariffs ToServerTariff(uint duration)
        {
            if (duration == 1)
                return DbrVpsServersTariffs.OneMonth;
            if (duration == 3)
                return DbrVpsServersTariffs.ThreeMonth;
            if (duration == 6)
                return DbrVpsServersTariffs.SixMonth;
            return DbrVpsServersTariffs.TwelveMonth;
        }
    }
}
