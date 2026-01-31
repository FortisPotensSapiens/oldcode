using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbLessons")]
    [Index(nameof(Number), nameof(CourseId), IsUnique = true)]
    public class AbLessonDbDto : EntityDbDtoBase
    {
        public AbCourseDbDto Course { get; set; }
        public Guid CourseId { get; set; }

        public string Name { get; set; } = null!;

        public uint Number { get; set; }

        public string Body { get; set; } = null!;
        public List<AbUserCourseDbDto> UserCourses { get; set; } = new();
    }
}
