namespace AleBotApi.Models.RDtos
{
    public class UserCourseLessonRDto
    {
        public string CourseName { get; set; } = null!;

        public string CourseDescription { get; set; } = null!;

        public byte[] CoursePhoto { get; set; } = null!;

        public bool CourseFree { get; set; }


        public Guid? LastLessonId { get; set; }

        public uint LessonsLearned { get; set; }


        public bool UserCourseCompleted { get; set; }


        public Guid? LessonId { get; set; }

        public string? LessonName { get; set; } = null!;

        public uint? LessonNumber { get; set; }

        public string? LessonBody { get; set; } = null!;
    }
}
