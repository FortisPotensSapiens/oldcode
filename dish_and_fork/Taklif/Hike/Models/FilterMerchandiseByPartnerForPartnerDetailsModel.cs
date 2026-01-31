using Hike.Models.Base;

namespace Hike.Models
{
    public class FilterMerchandiseByPartnerForPartnerDetailsModel : PaginationModel
    {
        [Required]
        public Guid PartnerId { get; set; }
        /// <summary>
        /// Показать товары которые продавец скрыл сам.
        /// </summary>
        public bool? ShowHidden { get; set; }
        /// <summary>
        /// Показать товары которые закончились на складе (которые распродали уже)
        /// </summary>
        public bool? ShowOutOfStock { get; set; }
        /// <summary>
        /// Показать товары которые сейчас на модерации у Администратора
        /// </summary>
        public bool? ShowOnModeration { get; set; }
        /// <summary>
        /// Показать те товары к которым есть замечания от админа (заблокированные)
        /// </summary>
        public bool? ShowBlockedByAdmin { get; set; }
    }
}
