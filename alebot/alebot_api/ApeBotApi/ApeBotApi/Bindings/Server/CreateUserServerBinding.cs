using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Server
{
    public class CreateUserServerBinding
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ServerId { get; set; }

        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;
    }
}
