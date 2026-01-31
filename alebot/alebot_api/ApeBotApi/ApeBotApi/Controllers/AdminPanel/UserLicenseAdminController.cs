using AleBotApi.Bindings.License;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Лицензии пользователя
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/user-license-admin")]
    public class UserLicenseAdminController : ControllerBase
    {
        private readonly ILogger<UserLicenseAdminController> _logger;
        private readonly AbDbContext _db;

        public UserLicenseAdminController(ILogger<UserLicenseAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список лицензий пользователя
        /// </summary>
        [HttpGet("user-licenses")]
        [ProducesResponseType(200, Type = typeof(List<AbUserLicenseDbDto>))]
        public async Task<IActionResult> GetUserLicenses(Guid? userId, Guid? licenseId, string? activationKey)
        {
            var query = _db.UserLicenses.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (licenseId.HasValue)
                query = query.Where(x => x.LicenseId == licenseId);

            if (!string.IsNullOrWhiteSpace(activationKey))
                query = query.Where(x => EF.Functions.ILike(x.ActivationKey, $"%{activationKey}%"));

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить лицензию пользователя
        /// </summary>
        [HttpGet("user-licenses/{userLicenseId}")]
        [ProducesResponseType(200, Type = typeof(AbUserLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserLicense(Guid userLicenseId)
        {
            var userLicense = await _db.UserLicenses.FirstOrDefaultAsync(x => x.Id == userLicenseId);
            if (userLicense == null)
                return NotFound($"Не найдена лицензия пользователя {userLicense}");

            return Ok(userLicense);
        }

        /// <summary>
        /// Создать лицензию пользователя
        /// </summary>
        [HttpPost("user-licenses")]
        [ProducesResponseType(200, Type = typeof(AbUserLicenseDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateUserLicense(CreateUserLicenseBinding binding)
        {
            var userLicense = await _db.UserLicenses.FirstOrDefaultAsync(x => x.UserId == binding.UserId
                && x.LicenseId == binding.LicenseId
                && x.ActivationKey == binding.ActivationKey);
            if (userLicense != null)
                return ValidationProblem($"Уже есть лицензия {binding.LicenseId} пользователя {binding.UserId} с кодом активации {binding.ActivationKey}");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == binding.UserId);
            if (user == null)
                return NotFound($"Пользователь {binding.UserId} не найден");

            var license = await _db.Licenses.FirstOrDefaultAsync(x => x.Id == binding.LicenseId);
            if (license == null)
                return NotFound($"Лицензия {binding.LicenseId} не найдена");

            userLicense = new AbUserLicenseDbDto
            {
                UserId = binding.UserId,
                LicenseId = binding.LicenseId,
                ActivationKey = binding.ActivationKey,
                TradingAccount = binding.TradingAccount,
            };

            _db.UserLicenses.Add(userLicense);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUserLicense Exc");
                return UnprocessableEntity("Не удалось создать лицензию пользователя");
            }

            return await GetUserLicense(userLicense.Id);
        }

        /// <summary>
        /// Удалить лицензию пользователя
        /// </summary>
        [HttpDelete("user-licenses/{userLicenseId}")]
        [ProducesResponseType(200, Type = typeof(AbUserLicenseDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteUserLicense(Guid userLicenseId)
        {
            var userLicense = await _db.UserLicenses.FirstOrDefaultAsync(x => x.Id == userLicenseId);
            if (userLicense == null)
                return NotFound($"Не найдена лицензия пользователя {userLicenseId}");

            _db.UserLicenses.Remove(userLicense);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUserLicense Exc");
                return UnprocessableEntity("Не удалось удалить лицензию пользователя");
            }

            return Ok();
        }
    }
}
