
namespace Hike.Entities
{
    public class ShopDto : EntityDtoBase
    {
        public List<MerchandiseDto> Goods { get; set; } = new List<MerchandiseDto>();
        public List<OfferToApplicationDto> Offers { get; set; } = new List<OfferToApplicationDto>();
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public PartnerDto Partner { get; set; }
        public Guid PartnerId { get; set; }
        public List<RatingDto> Ratings { get; set; } = new();

        //public ShopDto(Partner p)
        //{
        //    Id = p.Id;
        //    var patner = new PartnerDto(p);
        //    Partner = patner;
        //    PartnerId = patner.Id;
        //}

        //public ShopDto()
        //{

        //}

        //public void Applay(Partner p) => this.Partner.Applay(p);
    }
}
