using System;
using System.Linq;
using System.Security.Claims;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Models.RDtos;
using AleBotApi.Services;
using ApeBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers
{
    /// <summary>
    /// Рефералы которых пригласил на сайт пользователь
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/referrals")]
    public class ReferralsController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly AbDbContext _db;

        public ReferralsController(ILogger<WalletController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить информацию о рефералах текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(MyReferralsRDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetMyReferralsAsync()
        {
            var queryReferrer = from r in _db.Users
                                join u in _db.Users on r.Id equals u.Id
                                select new ReferrerRDto
                                {
                                    Id = r.Id,
                                    FullName = u.FullName,
                                    Email = u.Email,
                                    Level1 = from u1 in _db.Users where u1.RefererId == r.Id select u1,
                                    Level2 = from u1 in _db.Users
                                        join u2 in _db.Users on u1.Id equals u2.RefererId
                                        where u1.RefererId == r.Id
                                        select u2,
                                    Level3 = from u1 in _db.Users
                                        join u2 in _db.Users on u1.Id equals u2.RefererId
                                        join u3 in _db.Users on u2.Id equals u3.RefererId
                                        where u1.RefererId == r.Id
                                        select u3,
                                    MerchIds = from o in _db.Orders
                                        join ol in _db.OrderLines on o.Id equals ol.OrderId
                                        where o.UserId == r.Id
                                        select ol.MerchId,
                                    AccrualByReferalTransactions = from t in _db.AccountTransactions
                                        join a in _db.Accounts on t.AccountId equals a.Id
                                        where a.UserId == CurrentUserId && t.ByReferalId == r.Id
                                            && t.Reason == AccountTransactionReason.AccrualByReferal
                                        select t.Amount
                                };

            var referrer = await queryReferrer.FirstOrDefaultAsync(x => x.Id == CurrentUserId);
            if (referrer == null)
                return NotFound("Реферрер не найден");

            var referrals = referrer.Level1
                .Union(referrer.Level2)
                .Union(referrer.Level3)
                .Select(x => queryReferrer.First(y => y.Id == x.Id))
                .Select(x => new MyReferralsReferralRDto
                {
                    FullName = x.FullName,
                    Email = x.Email,
                    MyIncome = x.MyIncome,
                    Total = x.Total,
                    Active = x.Level1Active,
                    ProductCount = x.ProductCount
                })
                .ToList();

            var result = new MyReferralsRDto
            {
                Total = referrer.Total,
                Active = referrer.Active,
                FirstLine = referrer.Level1.Count(),
                SecondLine = referrer.Level2.Count(),
                Referrals = referrals
            };

            return Ok(result);
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Current user is not defined"));

    }
}
