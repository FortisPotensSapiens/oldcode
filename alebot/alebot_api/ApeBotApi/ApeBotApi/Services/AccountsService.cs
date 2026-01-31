using AleBotApi.DbDtos;
using AleBotApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Services.ServiceBindings.EmailService;
using System.Diagnostics;
using ApeBotApi.DbDtos;
using AleBotApi.Clients;
using AleBotApi.EventsAndSagas;
using ApeBotApi.Extensions;

namespace AleBotApi.Services
{

    public interface IAccountsService
    {
        /// <summary>
        /// Применяет оплаченные транзации.
        /// </summary>
        Task CompletePaindTransactionsAsync();

        /// <summary>
        /// Применяет оплаченные заказы.
        /// </summary>
        Task CompletePaindOrdersAsync();

        /// <summary>
        /// Выполянет оплаченную транзацию.
        /// Без сохранения в БД, метод сохранения нужно вызывать отдельно.
        /// </summary>
        Task CompletePaindTransactionAsync(AbAccountTransactionDbDto transaction);

        /// <summary>
        /// Выполянет оплаченный заказ.
        /// Без сохранения в БД, метод сохранения нужно вызывать отдельно.
        /// </summary>
        Task CompletePaindOrderAsync(AbOrderDbDto order);
    }

    public class AccountsService : IAccountsService
    {
        private readonly AbDbContext _db;
        private readonly ILogger<AccountsService> _logger;
        private readonly ICryptoCloudService _cryptoCloudService;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IEmailService _emailService;
      //  private readonly IDarkbandRuClient _darkband;

        public AccountsService(
            AbDbContext db,
            ILogger<AccountsService> logger,
            ICryptoCloudService cryptoCloudService,
            IHostApplicationLifetime lifetime,
            IEmailService emailService
         //   IDarkbandRuClient darkband
            )
        {
            _db = db;
            _logger = logger;
            _cryptoCloudService = cryptoCloudService;
            _lifetime = lifetime;
         //   _darkband = darkband;
            _emailService = emailService;
        }

        public async Task CompletePaindTransactionsAsync()
        {
            var skip = 0;
            var cancellationToken = _lifetime.ApplicationStopping;
            while (!cancellationToken.IsCancellationRequested)
            {
                var transactions = await _db.AccountTransactions
                    .Where(x => x.State == AccountTransactionState.Created)
                    .Where(x => x.ExternalId != null)
                    .OrderByDescending(x => x.Created)
                    .Skip(skip)
                    .Take(1000)
                    .ToListAsync(cancellationToken);
                skip += 1000;
                if (transactions.Count == 0)
                    break;

                foreach (var transaction in transactions)
                {
                    try
                    {
                        Debug.Assert(transaction != null);
                        Debug.Assert(transaction.ExternalId != null);

                        var isCompleited = await _cryptoCloudService.IsInvocePaind(transaction.ExternalId.ToString());
                        if (!isCompleited)
                            continue;

                        await CompletePaindTransactionAsync(transaction);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error on foreach run with transaction {transaction?.Id}");
                    }
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task CompletePaindOrdersAsync()
        {
            var skip = 0;
            var cancellationToken = _lifetime.ApplicationStopping;
            while (!cancellationToken.IsCancellationRequested)
            {
                var orders = await _db.Orders
                    .Where(x => x.State == OrderState.Created)
                    .Where(x => x.ExternalId != null)
                    .OrderByDescending(x => x.Created)
                    .Skip(skip)
                    .Take(1000)
                    .ToListAsync(cancellationToken);
                skip += 1000;

                if (orders.Count == 0)
                    break;

                foreach (var order in orders)
                {
                    try
                    {
                        Debug.Assert(order != null);
                        Debug.Assert(order.ExternalId != null);

                        var isCompleited = await _cryptoCloudService.IsInvocePaind(order.ExternalId.ToString());
                        if (!isCompleited)
                            continue;

                        await CompletePaindOrderAsync(order);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error on foreach run with order {order?.Id}");
                    }
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task CompletePaindTransactionAsync(AbAccountTransactionDbDto transaction)
        {
            var account = await _db.Accounts.FindAsync(transaction.AccountId)
                ?? throw new Exception("Счёт не найден");

            account.Amount += transaction.Amount;
            transaction.State = AccountTransactionState.Completed;
            if (transaction.Reason == AccountTransactionReason.AccrualByCryptoCloud)
            {
                try
                {
                    var user = await _db.Users.FindAsync(account.UserId);
                    await _emailService.SendUserAccuredWalletNotifyToAdmin(user.Id, user.UserName, transaction.Id, transaction.Amount);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on SendUserAccuredWalletNotifyToAdmin ");
                }
            }
        }

        public async Task CompletePaindOrderAsync(AbOrderDbDto order)
        {
            CancellationToken cancellationToken = _lifetime.ApplicationStopping;
            var orderLines = await _db.OrderLines.Where(x => x.OrderId == order.Id).ToListAsync(cancellationToken: cancellationToken);
            if (orderLines == null || orderLines.Count < 1)
                throw new Exception("Состав заказа не определен");

            var user = await _db.Users.FindAsync(order.UserId)
                ?? throw new Exception("Пользоваетль не найден");

            order.State = OrderState.Paid;
            order.PurchasedOn = DateTime.UtcNow;
            _db.EventsAndSagas.Add(new AbEventOrSagaDbDto { Type = AbEventOrSagaType.OrderPaid, Id = Guid.NewGuid(), Body = new OrderPaid { OrderId = order.Id, UserId = user.Id }.ToJson() });
            await ApplyOrderLinesAsync(order);
            await RefillsReferrersAsync(order, user);

        }

        private async Task ApplyOrderLinesAsync(AbOrderDbDto order)
        {
            var orderLines = await _db.OrderLines.Where(x => x.OrderId == order.Id).ToListAsync();
            if (orderLines.Count < 1)
                return;

            foreach (var orderLine in orderLines)
            {
                var merch = await _db.Merches
                    .Include(x => x.MerchServers)
                    .ThenInclude(x => x.Server)
                    .Include(x => x.MerchLicenses)
                    .Include(x => x.MerchServerExtentions)
                    .ThenInclude(x => x.ServerExtention)
                    .FirstOrDefaultAsync(x => x.Id == orderLine.MerchId);
                if (merch == null)
                    continue;

                var coursesIds = await _db.MerchCourses.Where(x => x.MerchId == merch.Id).Select(x => x.CourseId).ToListAsync();
                if (coursesIds.Count < 1)
                    continue;

                foreach (var courseId in coursesIds)
                {
                    var userCourse = await _db.UserCourses.FirstOrDefaultAsync(x => x.UserId == order.UserId && x.CourseId == courseId);
                    if (userCourse != null)
                        continue;

                    var course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
                    if (course == null)
                        continue;

                    userCourse = new AbUserCourseDbDto
                    {
                        UserId = order.UserId,
                        CourseId = courseId,
                        LessonsLearned = []
                    };

                    _db.UserCourses.Add(userCourse);
                }
            }
        }

        private async Task RefillsReferrersAsync(AbOrderDbDto order, AbUserDbDto user)
        {
            if (user.RefererId == null)
                return;

            await RefillsReferrerAsync(order, user, user.RefererId.Value, 1);
        }

        private async Task RefillsReferrerAsync(AbOrderDbDto order, AbUserDbDto user, Guid referrerId, int level)
        {
            var referrer = await _db.Users.FirstOrDefaultAsync(x => x.Id == referrerId);
            if (referrer == null)
            {
                _logger.LogWarning($"Не найден реферрер {referrerId}");
                return;
            }

            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.CurrencyId == order.CurrencyId && x.UserId == referrerId);
            if (account == null)
            {
                _logger.LogWarning($"Не найден счет реферала {referrerId} в валюте {order.CurrencyId}");
                return;
            }

            decimal k = 0;
            switch (level)
            {
                case 1:
                    k = .4M;
                    break;
                case 2:
                    k = .2M;
                    break;
                case 3:
                    k = .05M;
                    break;
                default:
                    _logger.LogWarning($"Некорректный уровень реферала");
                    return;
            }

            var transaction = new AbAccountTransactionDbDto
            {
                AccountId = account.Id,
                OperationType = AccountTransactionOperationType.Accrual,
                Reason = AccountTransactionReason.AccrualByReferal,
                State = AccountTransactionState.Completed,
                Amount = k * order.Amount,
                OperationDescription = $"Доход от реферала {user.Email} ({user.Id}) {level}-го уровня",
                ByReferalId = user.Id,
            };

            _db.AccountTransactions.Add(transaction);
            account.Amount += transaction.Amount;

            if (++level > 3 || referrer.RefererId == null)
                return;

            await RefillsReferrerAsync(order, user, referrer.RefererId.Value, level);
        }
    }
}
