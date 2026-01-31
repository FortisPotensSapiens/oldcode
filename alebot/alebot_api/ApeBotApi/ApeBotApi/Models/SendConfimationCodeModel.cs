using System.ComponentModel.DataAnnotations;
namespace AleBotApi.Models
{
    public sealed class SendConfimationCodeModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
