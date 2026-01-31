using AleBotApi.Bindings.Merch;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Лицензии продукта
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/merch-license-admin")]
    public class MerchLicenseAdminController : ControllerBase
    {
        private readonly ILogger<MerchLicenseAdminController> _logger;
        private readonly AbDbContext _db;

        public MerchLicenseAdminController(ILogger<MerchLicenseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список лицензий продукта
        /// </summary>
        [HttpGet("merch-licenses")]
        [ProducesResponseType(200, Type = typeof(List<AbMerchLicenseDbDto>))]
        public async Task<IActionResult> GetMerchLicenses(Guid? merchId, Guid? licenseId)
        {
            var query = _db.MerchLicenses.AsNoTracking();

            if (merchId.HasValue)
                query = query.Where(x => x.MerchId == merchId);

            if (licenseId.HasValue)
                query = query.Where(x => x.LicenseId == licenseId);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить лицензию продукта
        /// </summary>
        [HttpGet("merch-licenses/{merchLicenseId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetMerchLicense(Guid merchLicenseId)
        {
            var merchLicense = await _db.MerchLicenses.FirstOrDefaultAsync(x => x.Id == merchLicenseId);
            if (merchLicense == null)
                return NotFound($"Не найдена лицензия продукта {merchLicenseId}");

            return Ok(merchLicense);
        }

        /// <summary>
        /// Создать лицензию продукта
        /// </summary>
        [HttpPost("merch-licenses")]
        [ProducesResponseType(200, Type = typeof(AbMerchLicenseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateMerchLicense(CreateMerchLicenseBinding binding)
        {
            var merchLicense = await _db.MerchLicenses.FirstOrDefaultAsync(x => x.MerchId == binding.MerchId
                && x.LicenseId == binding.LicenseId);
            if (merchLicense != null)
                return ValidationProblem($"Уже есть лицензия {binding.LicenseId} продукта {binding.MerchId}");

            var merch = await _db.Merches.FirstOrDefaultAsync(x => x.Id == binding.MerchId);
            if (merch == null)
                return NotFound($"Продукт {binding.MerchId} не найден");

            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == binding.LicenseId);
            if (license == null)
                return NotFound($"Лицензия {binding.LicenseId} не найдена");

            merchLicense = new AbMerchLicenseDbDto
            {
                MerchId = binding.MerchId,
                LicenseId = binding.LicenseId,
                Qty = binding.Qty,
            };

            _db.MerchLicenses.Add(merchLicense);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateMerchLicense Exc");
                return UnprocessableEntity("Не удалось создать лицензию пользователя");
            }

            return await GetMerchLicense(merchLicense.Id);
        }

        /// <summary>
        /// Обновить лицензию продукта
        /// </summary>
        [HttpPatch("merch-licenses")]
        [ProducesResponseType(200, Type = typeof(AbMerchLicenseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeMerchLicense(ChangeMerchLicenseBinding binding)
        {
            var merchLicense = await _db.MerchLicenses.FirstOrDefaultAsync(x => x.Id == binding.MerchLicenseId);
            if (merchLicense == null)
                return NotFound($"Не найдена лицензия продукта {binding.MerchLicenseId}");

            if (merchLicense.Qty == binding.Qty)
                return await GetMerchLicense(merchLicense.Id);

            merchLicense.Qty = binding.Qty;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeMerchLicense Exc");
                return UnprocessableEntity("Не удалось обновить лицензию пользователя");
            }

            return await GetMerchLicense(merchLicense.Id);
        }

        /// <summary>
        /// Удалить лицензию продукта
        /// </summary>
        [HttpDelete("merch-licenses/{merchLicenseId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteMerchLicense(Guid merchLicenseId)
        {
            var merchLicense = await _db.MerchLicenses.FirstOrDefaultAsync(x => x.Id == merchLicenseId);
            if (merchLicense == null)
                return NotFound($"Не найдена лицензия продукта {merchLicenseId}");

            _db.MerchLicenses.Remove(merchLicense);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMerchLicense Exc");
                return UnprocessableEntity("Не удалось удалить лицензию продукта");
            }

            return Ok();
        }
    }
}
