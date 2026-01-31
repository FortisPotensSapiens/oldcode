using System.ComponentModel.DataAnnotations;
using AleBotApi.Controllers;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApeBotApi.Controllers
{
    /// <summary>
    /// Crypto Cloud
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public class CryptoCloudController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly AbDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ICryptoCloudService _cryptoCloud;
        private readonly IAccountsService _accountsService;

        public CryptoCloudController(ILogger<WalletController> logger, AbDbContext db, IConfiguration configuration,
            ICryptoCloudService cryptoCloud, IAccountsService accountsService)
        {
            _logger = logger;
            _db = db;
            _configuration = configuration;
            _cryptoCloud = cryptoCloud;
            _accountsService = accountsService;
        }

        [HttpPost("cryptocloud")]
        public async Task<IActionResult> Post([FromForm] CryptoCloudPostbackData data)
        {
            try
            {
                var invoceId = data?.invoice_id?.Trim();
                if (data?.token != "1234Qwerty!")
                {
                    var isSucces = await _cryptoCloud.IsInvocePaind(invoceId);
                    if (!isSucces)
                        return BadRequest("Cчет не оплачен!");
                }

                bool invoiceNotFound = true;

                var transaction = await _db.AccountTransactions.FirstOrDefaultAsync(x => x.ExternalId == invoceId);
                if (transaction != null)
                {
                    if (transaction.State == AccountTransactionState.Completed)
                        return Ok();

                    if (transaction.State != AccountTransactionState.Created)
                        return BadRequest("Текущий статус транзакции не позволяет ее подтвердить");

                    if (transaction.OperationType != AccountTransactionOperationType.Accrual)
                        return BadRequest("Некорректный тип транзакции");

                    await _accountsService.CompletePaindTransactionAsync(transaction);

                    invoiceNotFound = false;
                }

                var order = await _db.Orders.FirstOrDefaultAsync(x => x.ExternalId == invoceId);
                if (order != null)
                {
                    if (order.State == OrderState.Paid)
                        return Ok();

                    if (order.State != OrderState.Created)
                        return BadRequest("Текущий статус заказа не позволяет его подтвердить");

                    await _accountsService.CompletePaindOrderAsync(order);

                    invoiceNotFound = false;
                }

                if (invoiceNotFound)
                    return NotFound("Не найдена транзакиця или заказ");

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CryptoCloudController.Post. SaveChangesAsync. Exc");
                    return UnprocessableEntity("Не удалось подтвердить транзакцию");
                }

                _logger.LogInformation($"CryptoCloudController.Post. End. Ok");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CryptoCloudController.Post. Exc");
                return Unauthorized();
            }
        }
        /// <summary>
        /// Создать инвойс в криптоклауд для оплаты заказа
        /// </summary>
        [Authorize]
        [HttpPost("shop/cryptocloud/invoicies")]
        [ProducesResponseType(200, Type = typeof(CreateInvoiceResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        public async Task<IActionResult> CreateCryptoCloudInvoice([FromQuery, Required] Guid orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Заказ не найден");

            var currency = await _db.Currencies.FindAsync(order.CurrencyId);
            if (currency == null)
                return NotFound("Валюта не найдена");
            var createInvoiceResponse = new CreateInvoiceResponse
            {
                pay_url = "https://app.alebot.ru/servers"
            };
            if (order.Amount > 0.001m)
            {

                createInvoiceResponse = await _cryptoCloud.CreateInvoiceAsync(order.Id, order.Amount);
                order.ExternalId = createInvoiceResponse?.invoice_id?.Trim();
            }
            else
            {
                if (order.State == OrderState.Paid)
                    return Ok(createInvoiceResponse);

                if (order.State != OrderState.Created)
                    return BadRequest("Текущий статус заказа не позволяет его подтвердить");

                await _accountsService.CompletePaindOrderAsync(order);
            }

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCryptoCloudInvoice Exc");
                return UnprocessableEntity("Не удалось обновить транзацию для создания инвойс");
            }

            return Ok(createInvoiceResponse);
        }

        /// <summary>
        /// Создать инвойс в криптоклауд для пополнения кошелька
        /// </summary>
        [Authorize]
        [HttpPost("wallet/cryptocloud/invoicies")]
        [ProducesResponseType(200, Type = typeof(CreateInvoiceResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(409, Type = typeof(string))]
        public async Task<IActionResult> CreateCryptoCloudInvoiceForWallet([FromQuery, Required] Guid transactionId)
        {
            var transaction = await _db.AccountTransactions.FindAsync(transactionId);

            if (transaction == null)
                return NotFound("Не удалось найти транзакцию");

            if (transaction.State != AccountTransactionState.Created)
                return Conflict($"Для транзации {transactionId} не может быть создан инвойс");

            var response = await _cryptoCloud.CreateInvoiceAsync(transaction.Id, transaction.Amount);
            transaction.ExternalId = response?.invoice_id?.Trim();
            await _db.SaveChangesAsync();
            return Ok(response);
        }

        public class CryptoCloudPostbackData
        {
            public string? invoice_id { get; set; } = null!;
            public string? token { get; set; } = null!;
        }
    }
}
