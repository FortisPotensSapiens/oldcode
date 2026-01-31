using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AleBotApi.Bindings.Wallet;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Models.RDtos;
using AleBotApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers
{
    /// <summary>
    /// Кошелек
    /// </summary>
    [ApiController]
    [Route("api/v1/wallet")]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly AbDbContext _db;
        private readonly IEmailService _emailService;
        private readonly ICryptoCloudService _cryptoCloudService;

        public WalletController(ILogger<WalletController> logger, AbDbContext db, IEmailService emailService, ICryptoCloudService cryptoCloudService)
        {
            _logger = logger;
            _db = db;
            _emailService = emailService;
            _cryptoCloudService = cryptoCloudService;
        }

        /// <summary>
        /// Получить список доступных валют
        /// </summary>
        [AllowAnonymous]
        [HttpGet("currencies")]
        [ProducesResponseType(200, Type = typeof(List<AbCurrencyDbDto>))]
        public async Task<IActionResult> GetCurrencies()
        {
            return Ok(await _db.Currencies
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Получить список платежных систем
        /// </summary>
        [AllowAnonymous]
        [HttpGet("payment-systems")]
        [ProducesResponseType(200, Type = typeof(List<AbPaymentSystemDbDto>))]
        public async Task<IActionResult> GetPaymentSystems()
        {
            return Ok(await _db.PaymentSystems
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Получить список платежных сетей
        /// </summary>
        [AllowAnonymous]
        [HttpGet("payment-networks")]
        [ProducesResponseType(200, Type = typeof(List<AbPaymentNetworkDbDto>))]
        public async Task<IActionResult> GetPaymentNetworks()
        {
            return Ok(await _db.PaymentNetworks
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Создать счет текущему пользователю
        /// </summary>
        [Authorize]
        [HttpPost("accounts")]
        [ProducesResponseType(200, Type = typeof(AbAccountDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateAccount(Guid currencyId)
        {
            var currency = await _db.Currencies.FirstOrDefaultAsync(x => x.Id == currencyId);
            if (currency == null)
                return NotFound($"Не найдена валюта {currencyId}");

            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.CurrencyId == currencyId);
            if (account != null)
                return Ok(account);

            account = new AbAccountDbDto
            {
                UserId = CurrentUserId,
                CurrencyId = currencyId,
                Amount = 0,
            };

            _db.Accounts.Add(account);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount Exc");
                return UnprocessableEntity("Не удалось добавтить счёт");
            }

            return await GetAccount(account.Id);
        }

        /// <summary>
        /// Получить счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpGet("accounts")]
        [ProducesResponseType(200, Type = typeof(List<AbAccountDbDto>))]
        public async Task<IActionResult> GetCurrentAccounts()
        {
            return Ok(await _db.Accounts
                .Where(x => x.UserId == CurrentUserId)
                .AsNoTracking()
                .OrderBy(x => x.Created)
                .ToListAsync());
        }

        /// <summary>
        /// Получить данные счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpGet("accounts/{accountId}")]
        [ProducesResponseType(200, Type = typeof(AbAccountDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetAccount([FromRoute] Guid accountId)
        {
            var account = await _db.Accounts
                .Where(x => x.UserId == CurrentUserId && x.Id == accountId)
                .FirstOrDefaultAsync();

            if (account == null)
                return NotFound($"Счет {accountId} не найден");

            return Ok(account);
        }

        /// <summary>
        /// Получить список транзакий счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpGet("account/{accountId}/transactions")]
        [ProducesResponseType(200, Type = typeof(List<AccountTransactionModel>))]
        public async Task<IActionResult> GetAccountTransactions([FromRoute] Guid accountId,
            AccountTransactionOperationType? operationType = null,
            AccountTransactionState? transactionState = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
            => await GetTransactions(accountId, operationType, transactionState, dateFrom, dateTo);

        /// <summary>
        /// Получить список всех транзакий текущего пользователя
        /// </summary>
        [Authorize]
        [HttpGet("transactions")]
        [ProducesResponseType(200, Type = typeof(List<AccountTransactionModel>))]
        public async Task<IActionResult> GetTransactions(Guid? accountId = null,
            AccountTransactionOperationType? operationType = null,
            AccountTransactionState? transactionState = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            var query = _db.AccountTransactions
                .Join(_db.Accounts, t => t.AccountId, a => a.Id, (t, a) => new { AccountTransaction = t, Account = a })
                .Where(x => x.Account.UserId == CurrentUserId)
                .AsNoTracking();

            if (accountId.HasValue)
                query = query.Where(x => x.Account.Id == accountId.Value);

            if (operationType.HasValue)
                query = query.Where(x => x.AccountTransaction.OperationType == operationType);

            if (transactionState.HasValue)
                query = query.Where(x => x.AccountTransaction.State == transactionState);

            if (dateFrom.HasValue)
            {
                var d = dateFrom.Value.Date;
                query = query.Where(x => x.AccountTransaction.Created >= d);
            }

            if (dateTo.HasValue)
            {
                var d = dateTo.Value.Date.AddDays(1);
                query = query.Where(x => x.AccountTransaction.Created < d);
            }

            return Ok(await query
                .OrderByDescending(x => x.AccountTransaction.Created)
                .ToListAsync());
        }

        /// <summary>
        /// Создать транзакцию пополнение для счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpPost("transactions/accruals")]
        [ProducesResponseType(200, Type = typeof(AccountTransactionModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateAccountTransactionAccrual(CreateAccountTransactionAccrualBinding binding)
        {
            if (binding == null)
                return BadRequest("Не удалось прочитать запрос");

            if (binding.Amount < 0)
                return ValidationProblem("Сумма транзакции не может быть отрицательной");

            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == binding.AccountId);
            if (account == null)
                return NotFound($"Не найден счёт {binding.AccountId}");

            if (account.UserId != CurrentUserId)
                return Conflict($"Счёт {binding.AccountId} не принадлежит текущему пользователю {CurrentUserId}");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == account.UserId);
            if (user == null)
                return NotFound($"Не найден пользователь {account.UserId}");

            var transaction = new AbAccountTransactionDbDto
            {
                AccountId = binding.AccountId,
                OperationType = AccountTransactionOperationType.Accrual,
                Reason = AccountTransactionReason.AccrualByCryptoCloud,
                State = AccountTransactionState.Created,
                Amount = binding.Amount,
                PaymentSystemId = binding.PaymentSystemId,
                OperationDescription = "Пополнение счета"
            };

            _db.AccountTransactions.Add(transaction);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount Exc");
                return UnprocessableEntity("Не удалось добавтить счёт");
            }

            return await GetAccountTransaction(transaction.Id);
        }

        /// <summary>
        /// Создать транзакцию на вывод средств для счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpPost("transactions/debitings")]
        [ProducesResponseType(200, Type = typeof(AccountTransactionModel))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateAccountTransactionDebiting(CreateAccountTransactionDebitingBinding binding)
        {
            if (binding == null)
                return BadRequest("Не удалось прочитать запрос");

            if (binding.Amount < 0)
                return ValidationProblem("Сумма транзакции не может быть отрицательной");

            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == binding.AccountId);
            if (account == null)
                return NotFound($"Не найден счёт {binding.AccountId}");

            if (account.UserId != CurrentUserId)
                return Conflict($"Счёт {binding.AccountId} не принадлежит текущему пользователю {CurrentUserId}");

            if (account.Amount - binding.Amount < 0)
                return ValidationProblem($"На счёте {binding.AccountId} недостаточно средств");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == account.UserId);
            if (user == null)
                return NotFound($"Не найден пользователь {account.UserId}");

            var transaction = new AbAccountTransactionDbDto
            {
                AccountId = binding.AccountId,
                OperationType = AccountTransactionOperationType.Debiting,
                Reason = AccountTransactionReason.DebitingToCryptoCloud,
                State = AccountTransactionState.Created,
                Amount = binding.Amount,
                PaymentNetworkId = binding.PaymentNetworkId,
                DebitCryptoWalletAddress = binding.DebitCryptoWalletAddress,
                DebitCurrencyId = account.CurrencyId,
                OperationDescription = "Вывод средств"
            };

            _db.AccountTransactions.Add(transaction);

            account.Amount -= binding.Amount;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount Exc");
                return UnprocessableEntity("Не удалось добавтить счёт");
            }

            await _emailService.SendNewDebitingTransactionNotifyToAdmin(user.Id, user.Email, transaction.Id, transaction.Amount, transaction.DebitCryptoWalletAddress);

            return await GetAccountTransaction(transaction.Id);
        }

        /// <summary>
        /// Получить транзакию счета текущего пользователя
        /// </summary>
        [Authorize]
        [HttpGet("transactions/{transactionId}")]
        [ProducesResponseType(200, Type = typeof(List<AccountTransactionModel>))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetAccountTransaction([FromRoute] Guid transactionId)
        {
            var query = _db.AccountTransactions
                .Join(_db.Accounts, t => t.AccountId, a => a.Id, (t, a) => new { AccountTransaction = t, Account = a })
                .Where(x => x.Account.UserId == CurrentUserId && x.AccountTransaction.Id == transactionId)
                .AsNoTracking();

            var transaction = await query.FirstOrDefaultAsync();
            if (transaction == null)
                return NotFound("Не удалось найти транзакцию");

            return Ok(new AccountTransactionModel { Account = transaction.Account, AccountTransaction = transaction.AccountTransaction });
        }

        [Authorize]
        [HttpGet("my-income")]
        [ProducesResponseType(200, Type = typeof(MyIncomeRDto))]
        public async Task<IActionResult> MyIncome()
        {
            var accrualByReferalTransactions = from t in _db.AccountTransactions
                                               join a in _db.Accounts on t.AccountId equals a.Id
                                               where a.UserId == CurrentUserId && t.Reason == AccountTransactionReason.AccrualByReferal
                                               select t.Amount;

            var products = await accrualByReferalTransactions.SumAsync();
            var total = products;

            return Ok(new MyIncomeRDto { Total = total, Products = products });
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Current user is not defined"));
    }

    public class AccountTransactionModel
    {
        public AbAccountTransactionDbDto? AccountTransaction { get; set; }
        public AbAccountDbDto? Account { get; set; }
    }
}
