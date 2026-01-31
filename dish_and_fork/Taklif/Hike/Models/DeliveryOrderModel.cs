using Hike.Clients;
using Hike.Entities;

namespace Hike.Models
{
    public class DeliveryOrderModel
    {
        public required Guid OrderId { get; set; }



        public DostavistaCalculateOrderRequest ToDostavistaCalculateOrderRequest(OrderDto order, DeliveryInterval interval)
        {
            var packages = order.Items.Select(x => new DostavistaPakage
            {
                WareCode = x.Title,
                Description = $"Код товара: {x.Id} Описание: {x.Description}",
                ItemsCount = x.Amount.ToString(),
                ItemPaymentAmount = (x.Price?.Value ?? 0).ToString("##.##"),
                 // NomenclatureCode = x.Id.ToString()

            }).ToList();
            return new DostavistaCalculateOrderRequest
            {
                Matter = "Сладости",
                Type = DostavistaOrderType.standard,
                TotalWeightKg = (uint)Math.Ceiling(order.Items.Select(x => x.Amount * x.ServingGrossWeightInKilograms).Sum()),
                Points = new()
                {
                    new DostavistaPoint
                    {
                       Address = $"Москва, {order.SellerInfo.Address.Street}, {order.SellerInfo.Address.House}",
                       ContactPerson = new DostavistaContact{ Name = order.SellerInfo.Title, Phone = order.SellerInfo.ContactPhone},
                       Packages = packages,
                       Note= $"Москва, {order.SellerInfo.Address.Street}, {order.SellerInfo.Address.House}",
                       ClientOrderId  = order.Id.ToString()
                    },
                    new DostavistaPoint
                    {
                        Address = $"Москва, {order.RecipientAddress.Street}, {order.RecipientAddress.House}",
                        ContactPerson = new DostavistaContact{ Name = order.RecipientFullName, Phone = order.RecipientPhone},
                        Packages = packages,
                        Note = order.Comments,
                        RequiredStartDatetime = interval.RequiredStartDatetime,
                        RequiredFinishDatetime = interval.RequiredFinishDatetime,
                        ClientOrderId  = order.Id.ToString(),
                        ApartmentNumber = order.RecipientAddress.ApartmentNumber,
                         BuildingNumber = order.RecipientAddress.House,
                          EntranceNumber = order.RecipientAddress.Entrance,
                           FloorNumber = order.RecipientAddress.FloorNumber,
                            IntercomCode = order.RecipientAddress.Intercom
                    }
                },

            };
        }
    }
}
