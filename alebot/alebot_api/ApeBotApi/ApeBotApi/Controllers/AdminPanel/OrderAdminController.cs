using System.Reflection.PortableExecutable;
using System.Transactions;
using AleBotApi.Bindings;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Models.RDtos;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Заказы. 
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/order-admin")]
    public class OrderAdminController : ControllerBase
    {
        private readonly AbDbContext _db;

        public OrderAdminController(AbDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получить список уроков
        /// </summary>
        [HttpGet("orders")]
        [ProducesResponseType(200, Type = typeof(List<AbOrderDbDto>))]
        public async Task<IActionResult> GetOrders(Guid? orderId, Guid? userId, Guid? currencyId, Guid? paymentNetworkId,
            OrderState? orderState = null,
            DateTime? createdFrom = null,
            DateTime? createdTo = null,
            DateTime? purchasedFrom = null,
            DateTime? purchasedTo = null,
            decimal? amountFrom = null,
            decimal? amountTo = null,
            string? tradingAccount = null,
            string? externalId = null
            )
        {
            var query = _db.Orders.AsNoTracking();

            if (orderId.HasValue)
                query = query.Where(x => x.Id == orderId);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (currencyId.HasValue)
                query = query.Where(x => x.CurrencyId == currencyId);

            if (paymentNetworkId.HasValue)
                query = query.Where(x => x.PaymentNetworkId == paymentNetworkId);

            if (orderState.HasValue)
                query = query.Where(x => x.State == orderState);

            if (createdFrom.HasValue)
            {
                var d = createdFrom.Value.Date.ToUtc();
                query = query.Where(x => x.Created >= d);
            }

            if (createdTo.HasValue)
            {
                var d = createdTo.Value.Date.AddDays(1).ToUtc();
                query = query.Where(x => x.Created < d);
            }

            if (purchasedFrom.HasValue)
            {
                var d = purchasedFrom.Value.Date.ToUtc();
                query = query.Where(x => x.PurchasedOn >= d);
            }

            if (purchasedTo.HasValue)
            {
                var d = purchasedTo.Value.Date.AddDays(1).ToUtc();
                query = query.Where(x => x.PurchasedOn < d);
            }

            if (amountFrom.HasValue)
                query = query.Where(x => x.Amount >= amountFrom);

            if (amountTo.HasValue)
                query = query.Where(x => x.Amount <= amountTo);

            if (!string.IsNullOrWhiteSpace(tradingAccount))
                query = query.Where(x => x.TradingAccount != null && EF.Functions.ILike(x.TradingAccount, $"%{tradingAccount}%"));

            if (!string.IsNullOrWhiteSpace(externalId))
                query = query.Where(x => x.ExternalId != null && EF.Functions.ILike(x.ExternalId, $"%{externalId}%"));

            var resultQuery = from x in query
                              join u in _db.Users on x.UserId equals u.Id
                              select new OrderRDto
                              {
                                  OrderId = x.Id,
                                  UserId = u.Id,
                                  UserEmail = u.Email,
                                  CurrencyId = x.CurrencyId,
                                  PaymentNetworkId = x.PaymentNetworkId,
                                  TradingAccount = x.TradingAccount,
                                  PurchasedOn = x.PurchasedOn,
                                  Created = x.Created,
                                  State = x.State,
                                  Amount = x.Amount,
                                  ExternalId = x.ExternalId,
                                  Lines = from y in _db.OrderLines
                                          where y.OrderId == x.Id
                                          select new OrderLineRDto
                                          {
                                              OrderLineId = y.Id,
                                              MerchId = y.MerchId,
                                              Name = y.Name,
                                              Price = y.Price,
                                              CurrencyId = y.CurrencyId
                                          }
                              };

            return Ok(await resultQuery.AsNoTracking().OrderByDescending(x => x.Created).ToListAsync());
        }
    }
}
