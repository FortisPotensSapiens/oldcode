using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Server
{
    public class CreateServerBinding
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;
    }
}
