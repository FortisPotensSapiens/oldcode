using System.ComponentModel.DataAnnotations;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class AbUserDbDto : IdentityUser<Guid>
    {
        [MaxLength(10)]
        public string? EmailConfirmationCode { get; set; }
        [MaxLength(100)]
        public string? FullName { get; set; }
        [MaxLength(100)]
        public string? Telegram { get; set; }
        public byte[]? Photo { get; set; }
        public DateTime? EmailConfirmationSendedDate { get; set; }
        [MaxLength(10)]
        public string? ResetPasswordCode { get; set; }
        public AbUserDbDto? Referer { get; set; } = null;
        public Guid? RefererId { get; set; }
        public DateTime? LastActiveTime { get; set; }

        public DateTime? ResetPasswordSendedDate { get; set; }
        public List<UserRoleDbDto> Roles { get; set; } = new();
        public List<UserClaimDbDto> Claims { get; set; } = new();
        public List<UserLoginDbDto> Logins { get; set; } = new();
        public List<UserTokenDbDto> Tokens { get; set; } = new();
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public List<AbAccountDbDto> Accounts { get; set; } = new();
        public List<AbUserDbDto> Referals { get; set; } = new();
        public List<AbAccountTransactionDbDto> Transactions { get; set; } = new();
        public List<AbUserCourseDbDto> UserCourses { get; set; } = new();
        public List<AbUserLicenseDbDto> UserLicense { get; set; } = new();
        public List<AbUserServerDbDto> UserServers { get; set; } = new();
        public List<AbOrderDbDto> Orders { get; set; } = new();
        /// <summary>
        /// Id пользователя во внешней системе (В CRM)
        /// </summary>
        public string? ExternalId { get; set; }
        /// <summary>
        /// Параметры которы были в url регистраци. UTM теги, id реферера и прочее.
        /// </summary>
        public string? RegistrationQueryParams { get; set; }
    }
}
