using System.ComponentModel.DataAnnotations;
namespace AleBotApi.Models
{
    public sealed class ResetPasswordModel
    {
        [RegularExpression(@"^\d{6}$")]
        [Required]
        public string Code { get; set; }

        [EmailAddress]
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
