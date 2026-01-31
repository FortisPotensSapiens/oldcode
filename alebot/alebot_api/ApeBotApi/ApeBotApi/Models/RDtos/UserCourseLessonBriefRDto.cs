namespace AleBotApi.Models.RDtos
{
    public class UserCourseLessonBriefRDto
    {
        public Guid LessonId { get; set; }

        public string LessonName { get; set; } = null!;

        public uint LessonNumber { get; set; }

        public bool LessonLearned { get; set; }
    }
}
