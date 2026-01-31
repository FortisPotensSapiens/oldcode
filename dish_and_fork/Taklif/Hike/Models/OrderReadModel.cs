using Hike.Entities;

namespace Hike.Models
{
    public class OrderReadModel
    {
        public Guid Id { get; set; }
        public UserProfileReadModel Buyer { get; set; }
        public SellerShortInfoReadModel Seller { get; set; }
        public OrderState State { get; set; }
        public string RecipientPhone { get; set; }
        public string? Comments { get; set; }
        public AddressReadModel? RecipientAddress { get; set; } = new AddressReadModel();
        //  public PeriodModel DeliverDate { get; set; } = new PeriodModel();
        public List<OrderItemReadModel> Items { get; set; } = new List<OrderItemReadModel>();
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
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
        public decimal Amount { get; set; }
        /// <summary>
        /// Тип доставки
        /// </summary>
        public OrderDeliveryType DeliveryType { get; set; }

        public long Number { get; set; }
        public OrderType Type { get; set; }
        public string RecipientFullName { get; set; }
        public DeliveryOrderResponseModel DeliveryInfo { get; set; }
        public static OrderReadModel From(OrderDto dto, string baseUrl)
        {
            if (dto == null)
                return null;
            var df = dto.DeliveryInfo?.Clone();
            return new OrderReadModel()
            {
                Id = dto.Id,
                Buyer = UserProfileReadModel.From(dto.BuyerInfo),
                State = dto.State,
                RecipientPhone = dto.RecipientPhone,
                Comments = dto.Comments,
                RecipientAddress = AddressReadModel.From(dto.RecipientAddress),
                Items = dto.Items?.Select(x => OrderItemReadModel.From(x, baseUrl)).ToList(),
                Created = dto.Created,
                Updated = dto.Updated,
                PaymentDate = dto.PaymentDate,
                DeliveredDate = dto.DeliveredDate,
                Amount = dto.Amount.Value,
                DeliveryType = dto.DeliveryType,
                Number = dto.Number,
                Seller = new SellerShortInfoReadModel
                {
                    Id = dto.SellerInfo?.Id ?? Guid.Empty,
                    Title = dto.SellerInfo?.Title
                },
                Type = dto.Type,
                RecipientFullName = dto.RecipientFullName,
                DeliveryInfo = df == null ? null : new DeliveryOrderResponseModel(df),
            };
        }
    }
}
