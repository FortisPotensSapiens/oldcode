using System.ComponentModel.DataAnnotations;
using AleBotApi.Bindings.License;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Лицензии
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/license-admin")]
    public class LicenseAdminController : ControllerBase
    {
        private readonly ILogger<LicenseAdminController> _logger;
        private readonly AbDbContext _db;

        public LicenseAdminController(ILogger<LicenseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список лицензий
        /// </summary>
        [HttpGet("licenses")]
        [ProducesResponseType(200, Type = typeof(List<AbLicenseDbDto>))]
        public async Task<IActionResult> GetLicenses()
        {
            return Ok(await _db.Licenses
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Получить лицензию
        /// </summary>
        [HttpGet("licenses/{licenseId}")]
        [ProducesResponseType(200, Type = typeof(AbLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetLicense(Guid licenseId)
        {
            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == licenseId);
            if (license == null)
                return NotFound($"Не найдена лицензия {licenseId}");

            return Ok(license);
        }

        /// <summary>
        /// Создать лицензию
        /// </summary>
        [HttpPost("licenses")]
        [ProducesResponseType(200, Type = typeof(AbLicenseDbDto))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateLicense([FromBody][Required] CreateLicenseBinding binding)
        {
            var license = new AbLicenseDbDto
            {
                Name = binding.Name.Trim()
            };

            _db.Licenses.Add(license);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateLicense Exc");
                return UnprocessableEntity("Не удалось создать лицензию");
            }

            return await GetLicense(license.Id);
        }

        /// <summary>
        /// Изменить лицензию
        /// </summary>
        [HttpPatch("licenses")]
        [ProducesResponseType(200, Type = typeof(AbLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeLicense([FromBody][Required] ChangeLicenseBinding binding)
        {
            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == binding.LicenseId);
            if (license == null)
                return NotFound($"Не найдена лицензия {binding.LicenseId}");

            if (license.Name == binding.Name)
                return Ok(license);

            license.Name = binding.Name;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeLicense Exc");
                return UnprocessableEntity("Не удалось изменить лицензию");
            }

            return await GetLicense(license.Id);
        }

        /// <summary>
        /// Удалить лицензию
        /// </summary>
        [HttpDelete("licenses/{licenseId}")]
        [ProducesResponseType(200, Type = typeof(AbLicenseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteLicense(Guid licenseId)
        {
            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == licenseId);
            if (license == null)
                return NotFound($"Не найдена лицензия {licenseId}");

            var lessons = await _db.UserLicenses.Where(x => x.LicenseId == licenseId).ToListAsync();
            if (lessons.Count > 0)
                return ValidationProblem($"Не удалось удалить лицензию, так как у него есть лицензии пользователей");

            _db.Licenses.Remove(license);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteLicense Exc");
                return UnprocessableEntity("Не удалось удалить лицензию");
            }

            return Ok();
        }
    }
}
