using Hike.Clients;
using Hike.Entities;

namespace Hike.Models
{
    public class DeliveryIndividualPriceModel
    {
        [Required]
        public required Guid OfferId { get; set; }
        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Required]
        public AddressCreateModel RecipientAddress { get; set; }

        public DostavistaCalculateOrderRequest ToDostavistaCalculateOrderRequest(OfferToApplicationDto offer, DeliveryInterval interval)
        {
            return new DostavistaCalculateOrderRequest
            {
                Matter = "Сладости",
                Type = DostavistaOrderType.same_day,
                TotalWeightKg = (uint)Math.Ceiling(offer.ServingGrossWeightInKilograms),
                Points = new()
                {
                    new DostavistaPoint
                    {
                        Address = $"Москва, {offer.Shop.Partner.Address.Street}, {offer.Shop.Partner.Address.House}",
                        ContactPerson = new DostavistaContact{ Name = "Петя", Phone = "+79857428504"}
                    },
                    new DostavistaPoint
                    {
                        Address = $"Москва, {RecipientAddress.Street}, {RecipientAddress.House}",
                        ContactPerson = new DostavistaContact{ Name = "Вася", Phone = "+79857428505"},
                        RequiredStartDatetime = interval.RequiredStartDatetime,
                        RequiredFinishDatetime = interval.RequiredFinishDatetime
                    }
                }
            };
        }
    }
}
