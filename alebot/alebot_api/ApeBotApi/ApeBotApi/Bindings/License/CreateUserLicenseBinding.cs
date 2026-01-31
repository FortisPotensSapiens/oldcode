using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.License
{
    public class CreateUserLicenseBinding
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid LicenseId { get; set; }

        [Required]
        public string ActivationKey { get; set; } = null!;

        public string? TradingAccount { get; set; }
    }
}
