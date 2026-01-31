using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.License
{
    public class CreateLicenseBinding
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
