using Hike.Attributes;
using Hike.Entities;

namespace Hike.Models
{
    public class OrderSerfDeliveredCreateModel
    {
        public string RecipientFullName { get; set; }
        [Required, Phone, PhoneNumberValidation]
        public string RecipientPhone { get; set; }
        public string Comments { get; set; }
        [Required]
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();

        public OrderDto ToOrder(string buyerId, List<MerchandiseDto> merchandises)
        {
            var id = Guid.NewGuid();
            return new OrderDto()
            {
                Id = id,
                BuyerId = buyerId,
                Comments = Comments,
                RecipientPhone = RecipientPhone,
                Items = Items.Select(x => OrderItemDto.From(merchandises.FirstOrDefault(m => m.Id == x.ItemId), id, x.Amount)).Where(x => x != null).ToList(),
                State = OrderState.Created,
                DeliveryType = OrderDeliveryType.SelfDelivered,
                RecipientFullName = RecipientFullName
            };
        }
    }
}
