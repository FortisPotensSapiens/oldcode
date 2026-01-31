using Hike.Attributes;
using Hike.Entities;

namespace Hike.Models
{
    /// <summary>
    /// Создание индивидуального заказа c самодоставкой (приехать в магазин за товаром)
    /// </summary>
    public class OrderIndividualSelfDeliveredCreateModel
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
        /// Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ
        /// </summary>
        public Guid OfferId { get; set; }

        public OrderDto ToOrder(string buyerId, OfferToApplicationDto dto)
        {
            var id = Guid.NewGuid();
            return new OrderDto()
            {
                Id = id,
                BuyerId = buyerId,
                Comments = Comments,
                RecipientPhone = RecipientPhone,
                Items = new List<OrderItemDto>() { new OrderItemDto(dto, id) },
                State = OrderState.Created,
                DeliveryType = OrderDeliveryType.SelfDelivered,
                RecipientFullName = RecipientFullName
            };
        }
    }
}
