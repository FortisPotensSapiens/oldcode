using Hike.Entities;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class MerchRatingCreateModel : RatingCreateModelBase
        {
            public Guid MerchId { get; set; }
            public RatingDto ToMerchRating(string evaluatorId) => new RatingDto
            {
                Id = Guid.NewGuid(),
                RatingType = RatingType.Merch,
                Comment = Comment,
                EvaluatorId = evaluatorId,
                MerchandiseId = MerchId,
                Rating = Rating
            };
        }
    }
}
