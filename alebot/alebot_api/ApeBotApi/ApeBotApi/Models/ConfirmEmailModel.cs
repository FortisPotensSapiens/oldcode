using System.ComponentModel.DataAnnotations;
namespace AleBotApi.Models
{
    public sealed class ConfirmEmailModel
    {
        [RegularExpression(@"^\d{6}$")]
        [Required]
        public string Code { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
