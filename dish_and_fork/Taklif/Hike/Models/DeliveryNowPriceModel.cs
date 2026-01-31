using Hike.Clients;
using Hike.Entities;

namespace Hike.Models
{
    public class DeliveryNowPriceModel
    {
        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Required]
        public AddressCreateModel RecipientAddress { get; set; }
        /// <summary>
        /// Идетнификатор предложения на заявку (отклик на заявку на индивидуальный заказ
        /// </summary>
        [Required]
        public List<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();

        public DostavistaCalculateOrderRequest ToDostavistaCalculateOrderRequest(IEnumerable<MerchandiseDto> items, DeliveryInterval interval)
        {
            var pad = items.First().Shop.Partner.Address;
            var summ = 0.0;
            foreach (var item in items)
            {
                var amount = Items.FirstOrDefault(x => x.ItemId == item.Id)?.Amount;
                if (amount.HasValue)
                    summ += amount.Value * item.ServingGrossWeightInKilograms;
            }
            return new DostavistaCalculateOrderRequest
            {
                Matter = "Сладости",
                Type = DostavistaOrderType.same_day,
                TotalWeightKg = (uint)Math.Ceiling(summ),
                Points = new()
                {
                    new DostavistaPoint
                    {
                        Address = $"Москва, {pad.Street}, {pad.House}",
                        ContactPerson = new DostavistaContact{ Name = "Петя", Phone = "+79857428504"},
                    },
                    new DostavistaPoint
                    {
                        Address = $"Москва, {RecipientAddress.Street}, {RecipientAddress.House}",
                         ContactPerson = new DostavistaContact{ Name = "Вася", Phone = "+79857428505"},
                        RequiredStartDatetime = interval.RequiredStartDatetime,//?? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")),
                        RequiredFinishDatetime = interval.RequiredFinishDatetime // ?? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")).AddHours(3)
                    }
                }
            };
        }

    }
}
