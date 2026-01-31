using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbCourses")]
    public class AbCourseDbDto : EntityDbDtoBase
    {
        [MaxLength(1024)]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public byte[] Photo { get; set; } = null!;

        public bool Free { get; set; }
        public List<AbLessonDbDto> Lessons { get; set; } = new();
        public List<AbUserCourseDbDto> UserCourses { get; set; } = new();
        public List<AbMerchCourseDbDto> MerchCourses { get; set; } = new();
    }
}
