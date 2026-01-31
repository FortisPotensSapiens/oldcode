using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    public enum RatingType
    {
        Merch = 10,
        Partner
    }

[Index(nameof(EvaluatorId), nameof(MerchandiseId), nameof(ShopId), IsUnique = true)]
    public class RatingDto : EntityDtoBase
    {
        public MerchandiseDto Merchandise { get; set; }
        public Guid? MerchandiseId { get; set; }
        public ShopDto Shop { get; set; }
        public Guid? ShopId { get; set; }
        /// <summary>
        /// Оценивающий
        /// </summary>
        public HikeUser Evaluator { get; set; }
        [Required]
        public string EvaluatorId { get; set; }
        public RatingType RatingType { get; set; }
        [Required]
        public double Rating { get; set; }
        public string? Comment { get; set; }

        //public void Applay(RatingEntity entity)
        //{
        //    Rating = entity.Stars;
        //    Comment = entity.Comments;

        //}
        //public RatingDto(RatingEntity entity)
        //{
        //    if (entity is MerchRatingEntity)
        //        MerchandiseId = entity.EvaluatedId;
        //    if (entity is PartnerRatingEntity)
        //        ShopId = entity.EvaluatedId;
        //    EvaluatorId = entity.EvaluatorId;
        //    RatingType = entity.ToRatingType();
        //    Rating = entity.Stars;
        //    Comment = entity.Comments;
        //}

        //public RatingEntity ToRating() => RatingType.ToRating(
        //    Id,
        //    Rating,
        //    Comment,
        //    EvaluatorId,
        //    RatingType == RatingType.Merch ? MerchandiseId.Value : ShopId.Value,
        //    Created
        //    );

        //public RatingDto()
        //{

        //}
    }

    //public static class RatingTypeExtentions
    //{
    //    public static RatingType ToRatingType(this RatingEntity entity) => entity switch
    //    {
    //        MerchRatingEntity mre => RatingType.Merch,
    //        PartnerRatingEntity pre => RatingType.Partner,
    //        _ => throw new NotImplementedException()
    //    };

    //    public static RatingEntity ToRating(this RatingType type, RatingEntityId Id, RatingStars Stars, LongText? Comments, UserId EvaluatorId, EvaluatedId EvaluatedId, NotDefaultDateTime Created) => type switch
    //    {
    //        RatingType.Merch => new MerchRatingEntity(Id, Stars, Comments, EvaluatorId, EvaluatedId, Created),
    //        RatingType.Partner => new PartnerRatingEntity(Id, Stars, Comments, EvaluatorId, EvaluatedId, Created),
    //        _ => throw new NotImplementedException()
    //    };
    //}
}
