using Hike.Entities;

namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public class RatingUpdateModel : RatingCreateModelBase
        {
            public Guid Id { get; set; }
            public void Applay(RatingDto dto)
            {
                dto.Rating = Rating;
                dto.Comment = Comment;
            }
        }
    }
}
