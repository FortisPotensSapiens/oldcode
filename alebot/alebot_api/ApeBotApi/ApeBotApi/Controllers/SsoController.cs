using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using AleBotApi.Models;
using AleBotApi.Services;
using ApeBotApi.Attributes;
using ApeBotApi.DbDtos;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AleBotApi.Controllers
{

    [ApiController]
    [Route("api/v1/sso")]
    public class SsoController : ControllerBase
    {
        private readonly ILogger<SsoController> _logger;
        private readonly IEmailService _email;
        private readonly ICrmService _axl;
        private readonly AbDbContext _db;
        private readonly Microsoft.Extensions.Hosting.IApplicationLifetime _lifetime;
        private readonly UserManager<AbUserDbDto> _userManager;

        public SsoController(
            ILogger<SsoController> logger,
            AbDbContext db,
            Microsoft.Extensions.Hosting.IApplicationLifetime lifetime,
            UserManager<AbUserDbDto> userManager,
            IEmailService email,
            ICrmService axl
            )
        {
            _logger = logger;
            _email = email;
            _axl = axl;
            _db = db;
            _lifetime = lifetime;
            _userManager = userManager;
        }

        /// <summary>
        /// Получить данные текущего авторизованного пользователя
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("users/my-user-info")]
        [ProducesResponseType(200, Type = typeof(UserReadModel))]
        public async Task<UserReadModel?> GetMyUserInfo()
        {
            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _db.Users
                .Include(x => x.Roles)
                .ThenInclude(x => x.Role)
                .Include(x => x.Referer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return UserReadModel.From(user);
        }

        /// <summary>
        /// Зарегестрировать пользователя в системе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task Register(UserRegistrationModel model)
        {
            var ne = model.Email?.GetNormalizedName();
            if (await _db.Users.AnyAsync(x => x.NormalizedEmail == ne))
                return;
            var user = new AbUserDbDto
            {
                UserName = model.Email?.Trim(),
                Email = model.Email?.Trim(),
                RefererId = model.RefererId,
                NormalizedEmail = model.Email?.GetNormalizedName(),
                PhoneNumber = model.PhoneNumber?.GetNormalizedName(),
                RegistrationQueryParams = ParseQuery(model.RegistrationQueryParams?.Trim()),
                FullName = model.FullName?.Trim(),
            };
#if !DEBUG
            using var tran = await _db.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
#endif
            var r = await _userManager.CreateAsync(user, model.Password?.Trim());
            if (!r.Succeeded)
                new { r }.ThrowApplicationException("Не удалось выполнить регистрацию!");
            var courses = await _db.Courses.Where(x => x.Free)
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync();
            foreach (var courseId in courses)
            {
                var userCourse = new AbUserCourseDbDto
                {
                    UserId = user.Id,
                    CourseId = courseId,
                    LessonsLearned = []
                };
                _db.UserCourses.Add(userCourse);
            }
            var currency = await _db.Currencies.FirstOrDefaultAsync();
            if (currency != null)
            {
                var account = new AbAccountDbDto
                {
                    UserId = user.Id,
                    CurrencyId = currency.Id,
                    Amount = 0,
                };
                _db.Accounts.Add(account);
            }
            await _db.SaveChangesAsync();
#if !DEBUG
            await tran.CommitAsync();
#endif
        }

        /// <summary>
        /// Не чаше чем раз в миниту посылает по запросу на почту пользователя код для подтверждения email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-confirmation-code-to-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task SendConfirmationCodeToEmail(SendConfimationCodeModel model)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == model.Email.GetNormalizedName(), _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по email не найден!");
            if (user.EmailConfirmationSendedDate.HasValue && (DateTime.UtcNow - user.EmailConfirmationSendedDate.Value).TotalMinutes < 1)
                new { model }.ThrowApplicationException("Вы слишком часто посылаете писмо с кодом подтверждения!");
            var random = new Random();
            var code = $"{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}";
            user.EmailConfirmationCode = code;
            user.EmailConfirmationSendedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            await _email.SendEmailConfirmationCode(model.Email, code);
        }

        /// <summary>
        /// Метод подтверждает email пользователя если код верный.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("comfirm-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task ConfirmEmail(ConfirmEmailModel model)
        {
            var user = await _db.Users
               .FirstOrDefaultAsync(x => x.NormalizedEmail == model.Email.GetNormalizedName(), _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по email не найден!");
            if (user.EmailConfirmed)
                return;
            if (user.EmailConfirmationCode?.Trim() != model.Code?.Trim())
                new { model }.ThrowApplicationException("Не верный код подтверждения!");
            user.EmailConfirmed = true;
            await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            try
            {
                var id = await _axl.Create(user);
                user.ExternalId = id;
                await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при в AXL уведомления о регистрациия пользователя {@User}", user);
            }
            try
            {
                await _email.SendUserRegisteredEmailToAdmin(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке админам уведомления о регистрациия пользователя {@User}", user);
            }
        }

        /// <summary>
        /// Метод для начала процесса сброса пароля если пользователь забыл свой пароль. Посылает ему на почту код подтверждения для сброса пароля.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-password-reset-code-to-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task SendPasswordRestCodeToEmail(SendConfimationCodeModel model)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.NormalizedEmail == model.Email.GetNormalizedName(), _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по email не найден!");
            if (user.ResetPasswordSendedDate.HasValue && (DateTime.UtcNow - user.ResetPasswordSendedDate.Value).TotalMinutes < 1)
                new { model }.ThrowApplicationException("Вы слишком часто посылаете писмо с кодом сброса пароля!");
            var random = new Random();
            var code = $"{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}";
            user.ResetPasswordCode = code;
            user.ResetPasswordSendedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            await _email.SendPassworResetConfirmationCode(model.Email, code);
        }

        /// <summary>
        /// Метод для проверки что код для сброса пароля валидный.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>True или False в зависимости от того валиден ли код</returns>
        [HttpPost("check-password-reset-code-from-email")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> CheckPasswordResetCode(ConfirmEmailModel model)
        {
            var user = await _db.Users
               .FirstOrDefaultAsync(x => x.NormalizedEmail == model.Email.GetNormalizedName(), _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по email не найден!");
            if (user.ResetPasswordCode?.Trim() != model.Code?.Trim())
                return false;
            return true;
        }

        /// <summary>
        /// Устанавливает для пользователя новый пароль вместо старого
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await _db.Users
               .FirstOrDefaultAsync(x => x.NormalizedEmail == model.Email.GetNormalizedName(), _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по email не найден!");
            if (user.ResetPasswordCode?.Trim() != model.Code?.Trim())
                new { model }.ThrowApplicationException("Не верный код подтверждения!");
            await _userManager.RemovePasswordAsync(user);
            var r = await _userManager.AddPasswordAsync(user, model.Password);
            if (!r.Succeeded)
                new { r }.ThrowApplicationException("Не удалось задать новый пароль!");
        }

        /// <summary>
        /// Метод отправляет код подтверждения на новый email для его подтверждения когда пользователь хочет сменить email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("send-update-email-code-to-email")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task SendUpdateEmailCode(UserSendUpdateEmailCodeModel model)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == model.UserId, _lifetime.ApplicationStopping);
            if (user is null)
                new { model }.ThrowApplicationException("Пользователь по Id не найден!");
            if (user.EmailConfirmationSendedDate.HasValue && (DateTime.UtcNow - user.EmailConfirmationSendedDate.Value).TotalMinutes < 1)
                new { model }.ThrowApplicationException("Вы слишком часто посылаете писмо с кодом подтвреждения eamil!");
            var random = new Random();
            var code = $"{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}{random.Next(1, 9)}";
            user.EmailConfirmationCode = code;
            user.EmailConfirmationSendedDate = DateTime.UtcNow;
            await _db.SaveChangesAsync(_lifetime.ApplicationStopping);
            await _email.SendEmailConfirmationCode(model.NewEmail, code);
        }

        /// <summary>
        /// Обновить текущие данные пользователя.
        /// Если будет обновлен email то потребуеться так-же тут прислать код подтверждения email
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-my-user-info")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task UpdateUser(UserUpdateModel model)
        {
            var id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                new { id, model }.ThrowApplicationException("Не удалось найти пользователя для обновления");
            if (user.Email?.GetNormalizedName() != model.Email?.GetNormalizedName())
            {
                if (user.EmailConfirmationCode?.Trim() != model.Code?.Trim())
                    new { model }.ThrowApplicationException("Не верный код подтверждения email!");
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.GetNormalizedName();
                user.EmailConfirmed = true;
                user.UserName = user.Email;
                user.NormalizedUserName = user.NormalizedEmail;
            }
            user.FullName = model.FullName;
            user.Telegram = model.Telegram;
            user.PhoneNumber = model.PhoneNumber;
            user.Photo = model.Photo;
            await _db.SaveChangesAsync();
            try
            {
                await _axl.Update(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при в AXL уведомления о обновлении данных пользователя {@User}", user);
            }
        }

        /// <summary>
        /// Возвращает 200 ответ
        /// Если есть текущий пользователь, обновляет дату последней активности
        /// </summary>
        [AllowAnonymous]
        [HttpPost("ping")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> PingAsync()
        {
            var userIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdentifier != null)
            {
                var user = await _db.Users.FindAsync(Guid.Parse(userIdentifier));
                if (user == null)
                    return NotFound("Текущий пользователь не найден");

                user.LastActiveTime = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return Ok();
        }


        private string ParseQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return null;
            if (query.IndexOf('?') >= 0)
                query = query.Substring(query.IndexOf('?') + 1);
            var strs = query.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => Uri.UnescapeDataString(x))
                .ToList();
            return string.Join("; ", strs);
        }
    }

    public sealed class UserUpdateModel
    {
        [StringLength(100)]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 4)]
        public string Email { get; set; }
        [StringLength(20)]
        public string? Telegram { get; set; }
        /// <summary>
        /// Телефон пользователя с + и кодом страны аля +9929992222
        /// </summary>
        [PhoneNumberValidation]
        public string? PhoneNumber { get; set; }
        public byte[]? Photo { get; set; }
        /// <summary>
        /// Указывать только если поменяли email для его подтверждения.
        /// </summary>
        public string? Code { get; set; }

    }

    public class UserSendUpdateEmailCodeModel
    {
        /// <summary>
        /// Id пользователя которому меняем email
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Новый адресс почты
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string NewEmail { get; set; }
    }
}
