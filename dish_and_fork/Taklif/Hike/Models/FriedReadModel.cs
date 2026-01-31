namespace Hike.Models
{
    public class FriedReadModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstUserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string SecondUserId { get; set; }
        /// <summary>
        /// Если true то значит оба пользователя согласились добавить друг друга в друзья
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// Дата добавления в друзья
        /// </summary>
        public DateTime Created { get; set; }
    }
}
