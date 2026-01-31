using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AleBotApi.Bindings.Wallet;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Кошелек
    /// </summary>
    [ApiController]
    [Route("api/v1/wallet-admin")]
    public class WalletAdminController : ControllerBase
    {
        private readonly ILogger<WalletAdminController> _logger;
        private readonly AbDbContext _db;

        public WalletAdminController(ILogger<WalletAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Подтвердить транзакцию
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPut("transactions/{transactionId}/complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CompleteAccountTransaction([FromRoute, Required] Guid transactionId, [FromBody] CompleteTransactionModel model)
        {
            var transaction = await _db.AccountTransactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.State == AccountTransactionState.Created);
            if (transaction == null)
                return NotFound("Транзакция не найдена");

            if (transaction.OperationType != AccountTransactionOperationType.Debiting)
                return ValidationProblem("Администратор может закомплитить только транзакцию на вывод средств");

            transaction.State = AccountTransactionState.Completed;
            transaction.DebitFee = model.PaymentFee;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount Exc");
                return UnprocessableEntity("Не удалось подтвердить транзакцию");
            }

            return Ok();
        }

        /// <summary>
        /// Отменить транзакцию
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpPut("transactions/{transactionId}/cancel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CancelAccountTransaction(Guid transactionId)
        {
            var transaction = await _db.AccountTransactions
                .FirstOrDefaultAsync(x => x.Id == transactionId && x.State == AccountTransactionState.Created);
            if (transaction == null)
                return NotFound("Транзакция не найдена");

            var account = await _db.Accounts.FirstOrDefaultAsync(x => x.Id == transaction.AccountId);
            if (account == null)
                return NotFound("Счёт не найден");

            transaction.State = AccountTransactionState.Canceled;

            if (transaction.OperationType == AccountTransactionOperationType.Debiting)
                account.Amount += transaction.Amount;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateAccount Exc");
                return UnprocessableEntity("Не удалось отменить транзакцию");
            }

            return Ok();
        }

        /// <summary>
        /// Получить транзакию
        /// </summary>
        [Authorize(Roles = "admin")]
        [HttpGet("transactions/{transactionId}")]
        [ProducesResponseType(200, Type = typeof(AccountTransactionModel))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetAccountTransaction([FromRoute] Guid transactionId)
        {
            var query = _db.AccountTransactions
                .Join(_db.Accounts, t => t.AccountId, a => a.Id, (t, a) => new { AccountTransaction = t, Account = a })
                .Where(x => x.AccountTransaction.Id == transactionId)
                .AsNoTracking();

            var transaction = await query.FirstOrDefaultAsync();
            if (transaction == null)
                return NotFound("Не удалось найти транзакцию");

            return Ok(new AccountTransactionModel { Account = transaction.Account, AccountTransaction = transaction.AccountTransaction });
        }
    }

    public sealed class CompleteTransactionModel
    {
        [Required]
        [Range(0, 1000)]
        public decimal PaymentFee { get; set; }
    }
}
