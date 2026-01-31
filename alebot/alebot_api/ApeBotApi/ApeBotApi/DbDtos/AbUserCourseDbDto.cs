using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbUserCourses")]
    [Index(nameof(UserId), nameof(CourseId), IsUnique = true)]
    public class AbUserCourseDbDto : EntityDbDtoBase
    {
        public AbUserDbDto User { get; set; }
        public Guid UserId { get; set; }
        public AbCourseDbDto Course { get; set; }
        public Guid CourseId { get; set; }
        public AbLessonDbDto LastLesson { get; set; }
        public Guid? LastLessonId { get; set; }
        public Guid[] LessonsLearned { get; set; } = new Guid[0];
        public AbOrderLineDbDto? OrderLine { get; set; }
        public Guid? OrderLineId { get; set; }
    }
}
