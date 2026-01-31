using Hike.Entities;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class PartnerRatingCreateModel : RatingCreateModelBase
        {
            public Guid PartnerId { get; set; }
            public RatingDto ToPartnerRating(string evaluatorId) => new RatingDto
            {
                Id = Guid.NewGuid(),
                Rating = Rating,
                RatingType = RatingType.Partner,
                Comment = Comment,
                EvaluatorId = evaluatorId,
                ShopId = PartnerId                
            };
        }
    }
}
