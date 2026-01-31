using Hike.Entities;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class PartnerRatingReadModel : PartnerRatingCreateModel
        {
            public Guid Id { get; set; }
            public DateTime Created { get; set; }
            public string EvaluatorId { get; set; }
            public static PartnerRatingReadModel From(RatingDto dto)
            {
                if (dto == null) return null;
                return new PartnerRatingReadModel
                {
                    Id = dto.Id,
                    Created = dto.Created,
                    Rating = (byte)dto.Rating,
                    Comment = dto.Comment,
                    PartnerId = dto.ShopId.Value,
                    EvaluatorId = dto.EvaluatorId,
                };
            }
        }
    }
}
