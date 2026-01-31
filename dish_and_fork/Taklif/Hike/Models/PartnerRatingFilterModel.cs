using Hike.Models.Base;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class PartnerRatingFilterModel : PaginationModel
        {
            public Guid? PartnerId { get; set; }
        }
    }
}
