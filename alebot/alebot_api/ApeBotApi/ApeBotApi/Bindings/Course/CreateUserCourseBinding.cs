using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings
{
    public class CreateUserCourseBinding
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CourseId { get; set; }
    }
}
