using System.ComponentModel.DataAnnotations;
namespace AleBotApi.Models
{
    public sealed class CheckPasswordResetCodeModel
    {
        [RegularExpression(@"^\d{6}$")]
        [Required]
        public string Code { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
