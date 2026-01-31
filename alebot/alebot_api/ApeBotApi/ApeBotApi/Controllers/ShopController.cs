using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AleBotApi.Bindings.Order;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Models.RDtos;
using AleBotApi.Services;
using ApeBotApi.Controllers;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers
{
    /// <summary>
    /// Магазин
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/shop")]
    public class ShopController : ControllerBase
    {
        private readonly ILogger<ShopController> _logger;
        private readonly AbDbContext _db;
        private readonly ICryptoCloudService _cryptoCloudService;
        private readonly IWebHostEnvironment _environment;

        public ShopController(ILogger<ShopController> logger, AbDbContext db, ICryptoCloudService cryptoCloudService, IWebHostEnvironment environment)
        {
            _logger = logger;
            _db = db;
            _cryptoCloudService = cryptoCloudService;
            _environment = environment;
        }

        public class BuyedVdsServerRDto
        {
            public Guid MerchId { get; set; }
            public string ServerName { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string ServerAddress { get; set; }
            public DateTime Created { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public decimal MerchPrice { get; set; }
            public byte[] Photo { get; set; }
            public Guid UserServerId { get; set; }
        }

        public class BuyedLicenseRdto
        {
            public Guid MerchId { get; set; }
            public string LicenseName { get; set; }
            public string LicenseKey { get; set; }
            public string TradingAccountNumber { get; set; }
            public string MerchDescription { get; set; }
            public decimal MerchPrice { get; set; }
            public byte[] Photo { get; set; }
            public Guid UserLicenseId { get; set; }
        }

        public class NotBuyedMerchRDto
        {
            public Guid MerchId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public byte[] Photo { get; set; }
        }

        public class ServerVdsWithBuyedRDto
        {
            public List<BuyedVdsServerRDto> BuyedSevers { get; set; } = new();
            public List<NotBuyedMerchRDto> Merch { get; set; } = new();
        }

        public class LicenseWithBuyedRDto
        {
            public List<BuyedLicenseRdto> BuyedLicenses { get; set; } = new();
            public List<NotBuyedMerchRDto> Merch { get; set; } = new();
        }

        /// <summary>
        /// Получить список купленный и доступных к покупке серверов VDS для текущего пользователя
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(ServerVdsWithBuyedRDto))]
        [HttpGet("products/servers-vds")]
        public async Task<IActionResult> GetServersVds()
        {
            var se = await _db.Merches
                .AsNoTracking()
                .Where(x => x.MerchServerExtentions.Any())
                .Where(x => !x.MerchLicenses.Any())
                .Where(x => !x.MerchServers.Any())
                .Select(x => new { Merch = x, Duration = x.MerchServerExtentions.First().ServerExtention.ServerDurationInMonth })
                .ToListAsync();
            var servers = await _db.UserServers
               .AsNoTracking()
               .Include(x => x.Server)
               .Where(x => x.UserId == CurrentUserId)
               .ToListAsync();
            var buyed = new List<BuyedVdsServerRDto>();
            foreach (var item in servers)
            {
                var ml = se.FirstOrDefault(x => x.Duration == item.Server.ServerDurationInMonth);
                buyed.Add(new BuyedVdsServerRDto
                {
                    MerchId = ml?.Merch?.Id ?? Guid.Empty,
                    ServerName = item.Server.Name,
                    Login = item.Login,
                    Password = item.Password,
                    ServerAddress = item.Address,
                    Created = item.Created,
                    ExpirationDate = item.ExpirationDate,
                    MerchPrice = ml?.Merch?.Price ?? 0,
                    Photo = ml?.Merch?.Photo ?? new byte[] { 1, 2, 3, 4 },
                    UserServerId = item.Id
                });
            }
            var merch = await _db.Merches
             .AsNoTracking()
               .Where(x => !x.MerchServerExtentions.Any())
               .Where(x => !x.MerchLicenses.Any())
               .Where(x => x.MerchServers.Any())
            .Select(x => new NotBuyedMerchRDto
            {
                MerchId = x.Id,
                Name = x.Name,
                Description = x.ShortDescription,
                Price = x.Price,
                Photo = x.Photo ?? new byte[] { 1, 2, 3, 4 }
            })
             .OrderBy(x => x.Price)
            .ToListAsync();
            return Ok(new ServerVdsWithBuyedRDto
            {
                BuyedSevers = buyed,
                Merch = merch
            });
            //return Ok(new ServerVdsWithBuyedRDto
            //{
            //    BuyedSevers = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            //    .Select(x => new BuyedVdsServerRDto
            //    {
            //        MerchId = x,
            //        ServerName = "На 1 месяц",
            //        Login = "Trader",
            //        Password = x.ToString(),
            //        ServerAddress = "vps1.alebot.ru:44444",
            //        Created = DateTime.UtcNow,
            //        ExpirationDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            //        MerchPrice = 49M,
            //        Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 202.png"))
            //    })
            //    .ToList(),
            //    Merch = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            //    .Select(x => new NotBuyedMerchRDto
            //    {
            //        MerchId = x,
            //        Name = "На 1 месяц",
            //        Description = @"Вам будет выдан логин и пароль к виртуальному серверу на 1 месяц. После окончания срока действия сервер можно будет продлить.",
            //        Price = 49,
            //        Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 202.png"))
            //    })
            //    .ToList()
            //});
        }


        /// <summary>
        /// Получить список купленный и доступных к покупке продуктов с лицензией для текущего пользователя
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(LicenseWithBuyedRDto))]
        [HttpGet("products/with-licenses")]
        public async Task<IActionResult> GetWithLicenses()
        {

            var licenses = await _db.UserLicenses
                .AsNoTracking()
                .Include(x => x.License)
                .Where(x => x.UserId == CurrentUserId)
                .ToListAsync();
            var orderLines = await _db.MerchLicenses
                .Include(x => x.Merch)
                .AsNoTracking()
                .OrderBy(x => x.Merch.MerchServers.Any())
                .ToListAsync();
            var buyed = new List<BuyedLicenseRdto>();

            foreach (var item in licenses)
            {
                var ml = orderLines.FirstOrDefault(x => x.LicenseId == item.LicenseId)?.Merch;
                buyed.Add(new BuyedLicenseRdto
                {
                    MerchId = ml?.Id ?? Guid.Empty,
                    LicenseName = item.License.Name,
                    LicenseKey = item.ActivationKey,
                    TradingAccountNumber = item.TradingAccount,
                    MerchDescription = ml?.ShortDescription,
                    MerchPrice = ml?.Price ?? 0,
                    Photo = ml?.Photo ?? new byte[] { 1, 2, 3, 4 },
                    UserLicenseId = item.Id,
                });
            }

            var merch = await _db.Merches
                 .AsNoTracking()
                .Where(x => x.MerchServers.Any())
                .Where(x => x.MerchLicenses.Any())
                .Select(x => new NotBuyedMerchRDto
                {
                    MerchId = x.Id,
                    Name = x.Name,
                    Description = x.ShortDescription,
                    Price = x.Price,
                    Photo = x.Photo ?? new byte[] { 1, 2, 3, 4 }
                })
                .OrderBy(x => x.Price)
                .ToListAsync();
            return Ok(new LicenseWithBuyedRDto
            {
                BuyedLicenses = buyed,
                Merch = merch
            });

            //            return Ok(new LicenseWithBuyedRDto
            //            {
            //                BuyedLicenses = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            //                .Select(x => new BuyedLicenseRdto
            //                {
            //                    MerchId = x,
            //                    LicenseName = "Trial",
            //                    LicenseKey = "44444444",
            //                    TradingAccountNumber = "111140404",
            //                    MerchDescription = @"
            //- Демо версия   
            //- Доходность 10%   
            //- Минимальный депозит от 200%
            //",
            //                    MerchPrice = 44,
            //                    Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 202.png"))
            //                }).ToList(),
            //                Merch = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() }
            //                .Select(x => new NotBuyedMerchRDto
            //                {
            //                    MerchId = x,
            //                    Name = "Trial",
            //                    Description = @"
            //- Демо версия   
            //- Доходность 10%   
            //- Минимальный депозит от 200%
            //",
            //                    Price = 49,
            //                    Photo = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files", "Rectangle 202.png"))
            //                })
            //                .ToList()
            //            });
        }

        /// <summary>
        /// Получить список доступных для покупки продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet("products")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MerchBriefRDto>))]
        public async Task<IActionResult> GetAll()
        {
            var merchCoursesQuery = _db.MerchCourses.Join(_db.Courses, mc => mc.CourseId, c => c.Id, (mc, c) => new { mc.MerchId, Cource = c });
            var merchLicensesQuery = _db.MerchLicenses.Join(_db.Licenses, ml => ml.LicenseId, l => l.Id, (ml, l) => new { ml.MerchId, ml.Qty, License = l });
            var merchServersQuery = _db.MerchServers.Join(_db.Servers, ms => ms.ServerId, s => s.Id, (ms, s) => new { ms.MerchId, ms.Qty, Server = s });

            var query = from m in _db.Merches
                        join c in _db.Currencies on m.CurrencyId equals c.Id
                        select new MerchBriefRDto
                        {
                            Id = m.Id,
                            Currency = new MerchCurrencyRDto { CurrencyId = c.Id, Code = c.Code, Name = c.Name },
                            Name = m.Name,
                            ShortDescription = m.ShortDescription,
                            Photo = m.Photo,
                            Price = m.Price,
                            Courses = from x in merchCoursesQuery where x.MerchId == m.Id select new MerchCourseRDto { CourseId = x.Cource.Id, Name = x.Cource.Name },
                            Licenses = from x in merchLicensesQuery where x.MerchId == m.Id select new MerchLicenseRDto { LicenseId = x.License.Id, Name = x.License.Name, Qty = x.Qty },
                            Servers = from x in merchServersQuery where x.MerchId == m.Id select new MerchServerRDto { ServerId = x.Server.Id, Name = x.Server.Name, Qty = x.Qty },
                        };

            return Ok(await query.AsNoTracking().OrderBy(x => x.Price).ToListAsync());
        }

        /// <summary>
        /// Получить детальную информацию о продукте
        /// </summary>
        /// <returns></returns>
        [HttpGet("products/{id}")]
        [ProducesResponseType(200, Type = typeof(MerchRDto))]
        public async Task<IActionResult> Get([FromRoute, Required] Guid id)
        {
            var merchCoursesQuery = _db.MerchCourses.Join(_db.Courses, mc => mc.CourseId, c => c.Id, (mc, c) => new { mc.MerchId, Cource = c });
            var merchLicensesQuery = _db.MerchLicenses.Join(_db.Licenses, ml => ml.LicenseId, l => l.Id, (ml, l) => new { ml.MerchId, ml.Qty, License = l });
            var merchServersQuery = _db.MerchServers.Join(_db.Servers, ms => ms.ServerId, s => s.Id, (ms, s) => new { ms.MerchId, ms.Qty, Server = s });

            var query = from m in _db.Merches
                        join c in _db.Currencies on m.CurrencyId equals c.Id
                        where m.Id == id
                        select new MerchRDto
                        {
                            Id = m.Id,
                            Currency = new MerchCurrencyRDto { CurrencyId = c.Id, Code = c.Code, Name = c.Name },
                            Name = m.Name,
                            ShortDescription = m.ShortDescription,
                            FullDescription = m.FullDescription,
                            Photo = m.Photo,
                            Price = m.Price,
                        };

            var merch = await query.FirstOrDefaultAsync();
            if (merch == null)
                return NotFound($"Не найден продукт {id}");

            merch.Courses = merchCoursesQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchCourseRDto { CourseId = x.Cource.Id, Name = x.Cource.Name });
            merch.Licenses = merchLicensesQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchLicenseRDto { LicenseId = x.License.Id, Name = x.License.Name, Qty = x.Qty });
            merch.Servers = merchServersQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchServerRDto { ServerId = x.Server.Id, Name = x.Server.Name, Qty = x.Qty });

            return Ok(merch);
        }

        /// <summary>
        /// Получить список заказов текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet("orders")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AbOrderDbDto>))]
        public async Task<IActionResult> GetOrders()
        {
            var query = _db.Orders
                .AsNoTracking()
                .Where(x => x.UserId == CurrentUserId)
                .OrderByDescending(x => x.Created);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Создать заказ для оплаты. Метод возвращает Id созданного заказа
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        [HttpPost("orders")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderBinding binding)
        {
            var merch = await _db.Merches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == binding.MerchId);
            if (merch == null)
                return NotFound($"Не найден продукт {binding.MerchId}");
            var userId = User.IsInRole("admin") && binding.UserId.HasValue ? binding.UserId.Value : CurrentUserId;
            if (await _db.UserLicenses.AnyAsync(x => x.User.Id == userId && x.License.Name == "Trial"))
                new { binding, userId }.ThrowApplicationException("У вас уже есть триальная версия! Триальная версия может быть только одна и одни раз!");
            var user = await _db.Users.FindAsync(CurrentUserId);
            user.LastActiveTime = DateTime.UtcNow;
            var order = new AbOrderDbDto
            {
                Id = Guid.NewGuid(),
                UserId = User.IsInRole("admin") && binding.UserId.HasValue ? binding.UserId.Value : CurrentUserId,
                CurrencyId = merch.CurrencyId,
                PaymentNetworkId = merch.PaymentNetworkId,
                TradingAccount = binding.TradingAccount,
                State = OrderState.Created,
                Amount = merch.Price
            };

            var orderLine = new AbOrderLineDbDto
            {
                OrderId = order.Id,
                MerchId = merch.Id,
                Name = merch.Name,
                Price = merch.Price,
                CurrencyId = merch.CurrencyId,
                ExtendedUserServerId = binding.UserServerId
            };

            _db.Orders.Add(order);
            _db.OrderLines.Add(orderLine);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOrder Exc");
                return UnprocessableEntity("Не удалось создать заказ");
            }

            return Ok(order.Id);
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Current user is not defined"));
    }
}
