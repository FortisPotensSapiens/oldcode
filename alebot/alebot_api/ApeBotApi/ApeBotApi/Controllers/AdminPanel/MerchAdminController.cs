using System.ComponentModel.DataAnnotations;
using AleBotApi.Bindings.Merch;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.Models.RDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Продукты
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/merch-admin")]
    public class MerchAdminController : ControllerBase
    {
        private readonly ILogger<MerchAdminController> _logger;
        private readonly AbDbContext _db;

        public MerchAdminController(ILogger<MerchAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список продуктов
        /// </summary>
        [HttpGet("merches")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MerchBriefRDto>))]
        public async Task<IActionResult> GetMerches()
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

            return Ok(await query.AsNoTracking().ToListAsync());
        }

        /// <summary>
        /// Получить продукт
        /// </summary>
        [HttpGet("merches/{merchId}")]
        [ProducesResponseType(200, Type = typeof(MerchRDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetMerch(Guid merchId)
        {
            var merchCoursesQuery = _db.MerchCourses.Join(_db.Courses, mc => mc.CourseId, c => c.Id, (mc, c) => new { mc.MerchId, Cource = c });
            var merchLicensesQuery = _db.MerchLicenses.Join(_db.Licenses, ml => ml.LicenseId, l => l.Id, (ml, l) => new { ml.MerchId, ml.Qty, License = l });
            var merchServersQuery = _db.MerchServers.Join(_db.Servers, ms => ms.ServerId, s => s.Id, (ms, s) => new { ms.MerchId, ms.Qty, Server = s });

            var query = from m in _db.Merches
                        join c in _db.Currencies on m.CurrencyId equals c.Id
                        where m.Id == merchId
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
                return NotFound($"Не найден продукт {merchId}");

            merch.Courses = merchCoursesQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchCourseRDto { CourseId = x.Cource.Id, Name = x.Cource.Name });
            merch.Licenses = merchLicensesQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchLicenseRDto { LicenseId = x.License.Id, Name = x.License.Name, Qty = x.Qty });
            merch.Servers = merchServersQuery.Where(x => x.MerchId == merch.Id).Select(x => new MerchServerRDto { ServerId = x.Server.Id, Name = x.Server.Name, Qty = x.Qty });

            return Ok(merch);
        }

        /// <summary>
        /// Создать продукт
        /// </summary>
        [HttpPost("merches")]
        [ProducesResponseType(200, Type = typeof(AbMerchDbDto))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateMerch([FromBody][Required] CreateMerchBinding binding)
        {
            var merch = new AbMerchDbDto
            {
                CurrencyId = binding.CurrencyId,
                PaymentNetworkId = binding.PaymentNetworkId,
                Name = binding.Name.Trim(),
                ShortDescription = binding.ShortDescription.Trim(),
                FullDescription = binding.FullDescription.Trim(),
                Photo = binding.Photo,
                Price = binding.Price,
            };

            _db.Merches.Add(merch);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateMerch Exc");
                return UnprocessableEntity("Не удалось создать продукт");
            }

            return await GetMerch(merch.Id);
        }

        /// <summary>
        /// Изменить продукт
        /// </summary>
        [HttpPatch("merches")]
        [ProducesResponseType(200, Type = typeof(AbMerchDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeMerch([FromBody][Required] ChangeMerchBinding binding)
        {
            var merch = await _db.Merches.FirstOrDefaultAsync(x => x.Id == binding.MerchId);
            if (merch == null)
                return NotFound($"Не найден продукт {binding.MerchId}");

            if (merch.CurrencyId == binding.CurrencyId
                && merch.Name == binding.Name
                && merch.ShortDescription == binding.ShortDescription
                && merch.FullDescription == binding.FullDescription
                && merch.Photo == binding.Photo
                && merch.Price == binding.Price)
                return Ok(merch);

            merch.CurrencyId = binding.CurrencyId;
            merch.Name = binding.Name;
            merch.ShortDescription = binding.ShortDescription;
            merch.FullDescription = binding.FullDescription;
            merch.Photo = binding.Photo;
            merch.Price = binding.Price;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeMerch Exc");
                return UnprocessableEntity("Не удалось изменить продукт");
            }

            return await GetMerch(merch.Id);
        }

        /// <summary>
        /// Удалить продукт
        /// </summary>
        [HttpDelete("merches/{merchId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteMerch(Guid merchId)
        {
            var merch = await _db.Merches.FirstOrDefaultAsync(x => x.Id == merchId);
            if (merch == null)
                return NotFound($"Не найден продукт {merchId}");

            var orderLinesCount = await _db.OrderLines.Where(x => x.MerchId == merchId).CountAsync();
            if (orderLinesCount > 0)
                return ValidationProblem($"Не удалось удалить продукт, так как у него есть позиции заказов");

            var merchCoursesCount = await _db.MerchCourses.Where(x => x.MerchId == merchId).CountAsync();
            if (merchCoursesCount > 0)
                return ValidationProblem($"Не удалось удалить продукт, так как у него есть связанные курсы");

            var merchLicensesCount = await _db.MerchLicenses.Where(x => x.MerchId == merchId).CountAsync();
            if (merchLicensesCount > 0)
                return ValidationProblem($"Не удалось удалить продукт, так как у него есть связанные лицензии");

            var merchServersCount = await _db.MerchServers.Where(x => x.MerchId == merchId).CountAsync();
            if (merchServersCount > 0)
                return ValidationProblem($"Не удалось удалить продукт, так как у него есть связанные сервера VDS");

            _db.Merches.Remove(merch);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMerch Exc");
                return UnprocessableEntity("Не удалось удалить продукт");
            }

            return Ok();
        }
    }
}
