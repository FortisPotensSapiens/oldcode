using Hike.Entities;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class MerchRatingReadModel : MerchRatingCreateModel
        {
            public Guid Id { get; set; }
            public DateTime Created { get; set; }
            public string EvaluatorId { get; set; }

            public static MerchRatingReadModel From(RatingDto dto)
            {
                if (dto == null)
                    return null;
                return new MerchRatingReadModel
                {
                    Id = dto.Id,
                    Created = dto.Created,
                    Rating = (byte)dto.Rating,
                    Comment = dto.Comment,
                    MerchId = dto.MerchandiseId.Value,
                    EvaluatorId = dto.EvaluatorId,
                };
            }

        }
    }
}
