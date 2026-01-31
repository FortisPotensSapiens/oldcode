namespace Hike.Models
{
    public class FavoriteReadModel
    {
        /// <summary>
        /// Идентификатор пользователя которой добавил этот ресторан в избранное
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Дата добавление ресторана в избранное
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Идентификатор ресторана
        /// </summary>
        public string RestaurantId { get; set; }
    }
}