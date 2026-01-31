using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.License
{
    public class ChangeLicenseBinding
    {
        [Required]
        public Guid LicenseId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = null!;
    }
}
