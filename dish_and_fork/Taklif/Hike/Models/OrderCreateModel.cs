using Hike.Attributes;
using Hike.Entities;

namespace Hike.Models
{
    public class OrderCreateModel
    {
        /// <summary>
        /// ФИО получателя
        /// </summary>
        public string RecipientFullName { get; set; }
        /// <summary>
        /// Телефонный номер получателя
        /// </summary>
        [Required, Phone, PhoneNumberValidation]
        public string RecipientPhone { get; set; }
        /// <summary>
        /// Коментарии к заказу. Как проехать и т. п.
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// Адрес получателя
        /// </summary>
        [Required]
        public AddressCreateModel RecipientAddress { get; set; }
        /// <summary>
        /// Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ
        /// </summary>
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
                DeliveryType = OrderDeliveryType.Now,
                RecipientAddress = RecipientAddress.ToAddress(),
                RecipientFullName = RecipientFullName
            };
        }
    }
}
