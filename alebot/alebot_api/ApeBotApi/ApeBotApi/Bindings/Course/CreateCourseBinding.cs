using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings
{
    public class CreateCourseBinding
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public byte[] Photo { get; set; } = null!;

        [Required]
        public bool Free { get; set; }
    }
}
