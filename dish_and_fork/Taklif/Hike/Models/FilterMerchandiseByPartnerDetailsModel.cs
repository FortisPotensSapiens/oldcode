using Hike.Models.Base;

namespace Hike.Models
{
    public class FilterMerchandiseByPartnerDetailsModel : PaginationModel
    {
        [Required]
        public Guid PartnerId { get; set; }
    }
}
