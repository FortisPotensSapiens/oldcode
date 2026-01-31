namespace AleBotApi.Models.RDtos
{
    public class UserCourseRDto
    {
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public byte[] CoursePhoto { get; set; } = null!;

        public bool CourseFree { get; set; }

        public Guid? LastLessonId { get; set; }

        public uint LessonsLearned { get; set; }
        public string Description { get; set; }
        public int TotalLessonsCount { get; set; }  
    }
}
