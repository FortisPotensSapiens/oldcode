

namespace Hike.Entities
{
    public class OfferToApplicationImageDto : DbDtoBase
    {
        public FileDto File { get; set; }
        public Guid FileId { get; set; }
        public OfferToApplicationDto offerToApplication { get; set; }
        public Guid OfferToApplicationId { get; set; }
    }

    public class OfferToApplicationDto : EntityDtoBase
    {
        public List<OfferToApplicationImageDto> Images { get; set; } = new List<OfferToApplicationImageDto>();
        public ApplicationDto Application { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime Date { get; set; }
        public MoneyDto Sum { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public ShopDto Shop { get; set; }
        public Guid ShopId { get; set; }
        public HikeUser Creator { get; set; }
        [Required]
        public string CreatorId { get; set; }
        public List<ChatMessageDto> Comments { get; set; } = new List<ChatMessageDto>();
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public long Number { get; set; }
        /// <summary>
        /// Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        public double ServingGrossWeightInKilograms { get; set; }
        public OrderDto? Order { get; set; }
        /// <summary>
        /// Заказ в котором купили эту заявку
        /// </summary>
        public Guid? OrderId { get; set; }
        //public OfferToApplicationDto()
        //{

        //}

        //public OfferToApplicationDto(Offer offer)
        //{
        //    Id = offer.Id;
        //    ApplicationId = offer.ApplicationId;
        //    Date = offer.ReadyDate;
        //    Sum = new MoneyDto(offer.Sum);
        //    Description = offer.Description;
        //    ShopId = offer.PartnerId;
        //    CreatorId = offer.CreatorId;
        //    ServingGrossWeightInKilograms = offer.ServingGrossWeightInKilograms;
        //}


        //public Offer ToOffer()
        //{
        //    return new Offer(Id, Date, ServingGrossWeightInKilograms, ShopId,
        //        OrderItems?.Select(x => new Daf.SharedModule.Domain.OrderId(x.OrderId)).ToList() ?? new(),
        //        Sum.ToMoney(),
        //        Description,
        //        CreatorId, Number, ApplicationId);
        //}

        //public void Applay(Offer from)
        //{
        //    ApplicationId = from.ApplicationId;
        //    Date = from.ReadyDate;
        //    Sum = new MoneyDto(from.Sum);
        //    Description = from.Description;
        //    ShopId = from.PartnerId;
        //    CreatorId = from.CreatorId;
        //    ServingGrossWeightInKilograms = from.ServingGrossWeightInKilograms;
        //}
    }
}
