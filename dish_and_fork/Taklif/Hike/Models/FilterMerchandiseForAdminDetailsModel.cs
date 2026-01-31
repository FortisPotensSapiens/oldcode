using Hike.Models.Base;

namespace Hike.Models
{
    public class FilterMerchandiseForAdminDetailsModel : PaginationModel
    {
        public bool? IsTagsAppovedByAdmin { get; set; }
        /// <summary>
        /// Товары первый раз на модерацииь
        /// </summary>
        public bool? IsNew { get; set; }
    }
}
