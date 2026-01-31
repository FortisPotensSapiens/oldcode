using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.License
{
    public class ChangeUserLicenseBinding
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        public string TradingAccount { get; set; } = null!;
    }
}
