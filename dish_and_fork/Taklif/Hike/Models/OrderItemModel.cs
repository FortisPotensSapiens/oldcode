using Hike.Entities;

namespace Hike.Models
{
    public class OrderItemModel
    {
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public uint Amount { get; set; }
        public OrderItemDto ToOrderItemDto(Guid orderId) => new OrderItemDto() {Id =Guid.NewGuid(), Amount = Amount, ItemId = ItemId, OrderId = orderId, Type = OrderItemType.Standard };
    }
}
