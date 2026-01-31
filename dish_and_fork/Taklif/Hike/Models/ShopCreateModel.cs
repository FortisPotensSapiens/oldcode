using Hike.Entities;

namespace Hike.Models
{
    /// <summary>
    /// Модель для создания профиля фрилансера или магазина
    /// </summary>
    public class ShopCreateModel
    {
        /// <summary>
        /// Краткое наименование магазина или фрилансера
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        /// <summary>
        /// Описание магазина или фрилансера
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Тип это Магазин (витрина) или Флилансер (портфолио)
        /// </summary>
        public ShopType Type { get; set; }
        /// <summary>
        /// Адрес откуда будут забирать товар курьеры (адресс магазина или фрилансера)
        /// </summary>
        [Required]
        public AddressCreateModel Address { get; set; }
        /// <summary>
        /// Картинка магазина или фрилансера
        /// </summary>
        public Guid ImageId { get; set; }
        /// <summary>
        /// Партнер которому принадлежит этот магазин или этот профиль фрилансера.
        /// </summary>
        public Guid ParnerId { get; set; }

        public ShopDto ToShopDto()
        {
            var id = Guid.NewGuid();
            var p = new ShopDto()
            {
                Id = id
            };
            return p;
        }
    }
}
