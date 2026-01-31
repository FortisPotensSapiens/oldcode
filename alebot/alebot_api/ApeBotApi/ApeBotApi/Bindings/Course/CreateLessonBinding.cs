using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings
{
    public class CreateLessonBinding
    {
        [Required]
        public Guid CourseId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [Range(1,10000)]
        public uint Number { get; set; }
        [Required]
        public string Body { get; set; } = null!;
    }
}
