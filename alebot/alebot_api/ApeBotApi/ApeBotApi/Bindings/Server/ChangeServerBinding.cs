using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Server
{
    public class ChangeServerBinding
    {
        [Required]
        public Guid ServerId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = null!;
    }
}
