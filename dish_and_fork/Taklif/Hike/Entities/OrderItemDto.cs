
using Hike.Extensions;

namespace Hike.Entities
{
    public class OrderItemDto : EntityDtoBase
    {
        public OrderDto Order { get; set; }
        public Guid OrderId { get; set; }
        public MerchandiseDto Item { get; set; }
        public Guid? ItemId { get; set; }
        public uint Amount { get; set; }
        public OrderItemType Type { get; set; }
        public OfferToApplicationDto Offer { get; set; }
        public Guid? OfferId { get; set; }
        /// <summary>
        /// Еденицы измерения 
        /// </summary>
        public MerchandiseUnitType? UnitTypeType { get; set; }
        /// <summary>
        /// Цена за еденицу измерения
        /// </summary>
        public MoneyDto? Price { get; set; } = new();
        public MerchandisesState? State { get; set; }
        /// <summary>
        /// Размер порции. (0.1  кг или 1 штука)
        /// </summary>
        public double? ServingSize { get; set; }
        /// <summary>
        /// Партнеры которым принадлежит этот товар
        /// </summary>
        public ShopDto Shop { get; set; }
        public Guid ShopId { get; set; }
        public string? Title { get; set; }
        public string? NormalizedTitle { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        public double ServingGrossWeightInKilograms { get; set; }
        /// <summary>
        /// Дата в которую должен быть доставле заказ
        /// </summary>
        public DateTime? OfferDeliveryDate { get; set; }

        //public OfferOrdemItem ToOffer() =>
        //    new OfferOrdemItem(Description,
        //        Price.ToMoney(),
        //        ShopId, ServingGrossWeightInKilograms,
        //        OfferId,
        //        OfferDeliveryDate);

        //public MerchOrderItem ToMerch() =>
        //    new MerchOrderItem(
        //        Description,
        //        (int)Amount,
        //        Price.ToMoney(),
        //        ShopId,
        //        ServingGrossWeightInKilograms,
        //        Title,
        //        ItemId,
        //        UnitTypeType.Value.ToSize(ServingSize.Value)
        //        );

        //public OrderItemDto(OfferOrdemItem dto, Guid orderId)
        //{
        //    Id = Guid.NewGuid();
        //    OrderId = orderId;
        //    OfferId = dto.OfferId;
        //    Amount = 1;
        //    Type = OrderItemType.Individual;
        //    Price = new MoneyDto(dto.Price);
        //    Title = $"Отклик {dto.Number}";
        //    NormalizedTitle = Title.GetNormalizedName();
        //    Description = dto.Description;
        //    ShopId = dto.PartnerId;
        //    State = MerchandisesState.Created;
        //    ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms;
        //    OfferDeliveryDate = dto.ReadyDate;
        //}

        //public OrderItemDto(MerchOrderItem dto, Guid orderId)
        //{
        //    Id = Guid.NewGuid();
        //    OrderId = orderId;
        //    ItemId = dto.ItemId;
        //    Type = OrderItemType.Standard;
        //    UnitTypeType = dto.ServingSize.ToMerchandiseUnitType();
        //    Price = new MoneyDto(dto.Price);
        //    State = MerchandisesState.Published;
        //    ServingSize = dto.ServingSize.Value;
        //    ShopId = dto.PartnerId;
        //    Title = dto.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Description = dto.Description;
        //    ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms;
        //    Amount = (uint)dto.Amount.Value;
        //}

        //public OrderItemDto(MerchandiseDto dto, Guid orderId)
        //{
        //    Id = Guid.NewGuid();
        //    OrderId = orderId;
        //    ItemId = dto.Id;
        //    Type = OrderItemType.Standard;
        //    UnitTypeType = dto.UnitTypeType;
        //    Price = dto.Price?.Clone();
        //    State = dto.State;
        //    ServingSize = dto.ServingSize;
        //    ShopId = dto.ShopId;
        //    Title = dto.Title;
        //    NormalizedTitle = dto.NormalizedTitle;
        //    Description = dto.Description;
        //    ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms;

        //}

        public OrderItemDto()
        {

        }

        public OrderItemDto(OfferToApplicationDto dto, Guid orderId)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            OfferId = dto.Id;
            Amount = 1;
            Type = OrderItemType.Individual;
            Price = dto.Sum?.Clone();
            Title = $"Отклик {dto.Number}";
            NormalizedTitle = $"Отклик {dto.Number}".GetNormalizedName();
            Description = dto.Description;
            ShopId = dto.ShopId;
            State = MerchandisesState.Created;
            ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms;
            OfferDeliveryDate = dto.Date;
        }

        public static OrderItemDto From(MerchandiseDto dto, Guid orderId, uint amount)
        {
            if (dto == null)
                return null;
            return new OrderItemDto
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ItemId = dto.Id,
                Amount = amount,
                Type = OrderItemType.Standard,
                UnitTypeType = dto.UnitTypeType,
                Price = dto.Price?.Clone(),
                State = dto.State,
                ServingSize = dto.ServingSize,
                ShopId = dto.ShopId,
                Title = dto.Title,
                NormalizedTitle = dto.NormalizedTitle,
                Description = dto.Description,
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms
            };
        }
    }
}
