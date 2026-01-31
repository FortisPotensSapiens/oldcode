global using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Hike.Modules.AdminSettings;

namespace Hike.Entities
{
    /// <summary>
    /// Тип заказа
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Обычный заказ
        /// </summary>
        Standard = 10,
        /// <summary>
        /// Индивидуальный заказ
        /// </summary>
        Individual = 11
    }

    public class OrderDto : EntityDtoBase
    {
        public HikeUser Buyer { get; set; }
        [Required]
        public string BuyerId { get; set; }
        /// <summary>
        /// Информация о покупателе
        /// </summary>
        [Column(TypeName = "jsonb")]
        public BuyerInfoDto BuyerInfo { get; set; }
        /// <summary>
        /// Информация о продавце
        /// </summary>
        [Column(TypeName = "jsonb")]
        public SellerInfoDto SellerInfo { get; set; }
        public OrderState State { get; set; }
        public string? RecipientPhone { get; set; }
        [StringLength(1000)]
        public string? Comments { get; set; }
        [Column(TypeName = "jsonb")]
        public AddressDto RecipientAddress { get; set; } = new();
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        /// <summary>
        /// Дата когда заказ был оплачен
        /// </summary>
        public DateTime? PaymentDate { get; set; }
        /// <summary>
        /// Дата когда заказ был доставлен
        /// </summary>
        public DateTime? DeliveredDate { get; set; }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public MoneyDto? Amount { get; set; }
        public string? RecipientFullName { get; set; }
        /// <summary>
        /// Информация о доставке заказа.
        /// </summary>
        [Column(TypeName = "jsonb")]
        public DeliveryOrderResponseDto DeliveryInfo { get; set; }
        public decimal GetTotalAmount()
        {
            if (Items.Count == 0)
                return 0;
            return Items.Select(x => x.Amount * (x.Price?.Value ?? 0)).Sum();
        }
        /// <summary>
        /// Как будет осуществляться доставка этого заказа
        /// </summary>
        public OrderDeliveryType DeliveryType { get; set; } = OrderDeliveryType.Now;
        /// <summary>
        /// Сквозной номер заказа в системе по которому его можно найти
        /// </summary>
        public long Number { get; set; }
        /// <summary>
        /// Тип заказа
        /// </summary>
        public OrderType Type { get; set; } = OrderType.Standard;
        [Column(TypeName = "jsonb")]
        public AdminSettingsDto AdminSettings { get; set; }
        /// <summary>
        /// Идентификатор заказа в платежной системе
        /// </summary>
        public string? ExternalIdInPaymentSystem { get; set; }
        /// <summary>
        /// Идентификатор заказа в системе доставки 
        /// </summary>
        public string? ExternalIdInDeliverySystem { get; set; }

        public void SetTotalPrice()
        {
            var dp = DeliveryInfo?.DeliveryPrice?.Value ?? 0;
            var summ = GetTotalAmount();
            if (AdminSettings is { })
            {
                dp = AdminSettings.CalculateDeliveryTotalPrice(dp);
                summ = AdminSettings.CalculateOrderTotalPrice(summ);
            }
            Amount = new MoneyDto { CurrencyType = CurrencyType.Rub, Value = summ + dp };
            if (Amount.Value < 0)
                Amount.Value = 0;
        }
        //public void Applay(Order order)
        //{
        //    State = order.State.ToOrderState();
        //    PaymentDate = order.GetPaymentDate();
        //    DeliveredDate = order.GetDeliveredDate();
        //    Amount = new MoneyDto(order.Amount);
        //    var df = order.GetDeliveryInfo();
        //    if (df != null)
        //        DeliveryInfo = new DeliveryOrderResponseDto(df);
        //}

        //public OrderDto(Order entity)
        //{
        //    Id = entity.Id;
        //    BuyerId = entity.Buyer.Id;
        //    BuyerInfo = new BuyerInfoDto(entity.Buyer);
        //    SellerInfo = new SellerInfoDto(entity.Seller);
        //    State = entity.State.ToOrderState();
        //    RecipientPhone = entity.RecipientPhone;
        //    Comments = entity.Comments;
        //    var address = entity.GetDeliveryAddress();
        //    if (address != null)
        //        RecipientAddress = new AddressDto(address);
        //    if (entity.GetItems() != null)
        //    {
        //        Items.AddRange(entity.GetItems().Value.Select(x => new OrderItemDto(x, entity.Id)));
        //    }
        //    else
        //    {
        //        Items.Add(new OrderItemDto(entity.GetOffer(), entity.Id));
        //    }
        //    Amount = new MoneyDto(entity.Amount);
        //    RecipientFullName = entity.RecipientFullName;
        //    DeliveryType = entity.DeliveryType.ToDeliveryType();
        //    Type = entity.ToOrderType();
        //}

        //public OrderDto()
        //{

        //}
    }
}
