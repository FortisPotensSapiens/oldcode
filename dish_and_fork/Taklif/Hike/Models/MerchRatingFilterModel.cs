using Hike.Models.Base;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class MerchRatingFilterModel : PaginationModel
        {
            public Guid? MerchId { get; set; }
        }
    }
}
