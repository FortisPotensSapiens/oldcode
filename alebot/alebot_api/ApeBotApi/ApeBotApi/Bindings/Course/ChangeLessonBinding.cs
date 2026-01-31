using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings
{
    public class ChangeLessonBinding
    {
        [Required]
        public Guid LessonId { get; set; }
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
