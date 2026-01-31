using Hike.Models.Base;

namespace Hike.Models
{
    public class CategoryFilterModel : PaginationModel
    {
        public string Title { get; set; }
        /// <summary>
        /// Скрывать категории без товаров
        /// </summary>
        public bool HideEmpty { get; set; }
    }
}
