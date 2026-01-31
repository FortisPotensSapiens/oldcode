namespace Hike.Controllers
{
    public partial class RatingsController
    {
        public abstract class RatingCreateModelBase
        {
            public string? Comment { get; set; }
            [Required]
            [Range(1, 5)]
            public byte Rating { get; set; }
        }
    }
}
