using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings
{
    public class ChangeCourseBinding
    {
        [Required]
        public Guid CourseId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public byte[] Photo { get; set; } = null!;

        [Required]
        public bool Free { get; set; }
    }
}
