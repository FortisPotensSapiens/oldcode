using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class CreateMerchCourseBinding
    {
        [Required]
        public Guid MerchId { get; set; }

        [Required]
        public Guid CourseId { get; set; }
    }
}
