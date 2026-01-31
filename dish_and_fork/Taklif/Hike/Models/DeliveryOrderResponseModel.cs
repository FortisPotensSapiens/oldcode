using Hike.Clients;
using Hike.Entities;

namespace Hike.Models
{
    public class DeliveryOrderResponseModel
    {
        /// <summary>
        /// Цена доставки
        /// </summary>
        public decimal DeliveryPrice { get; set; }
        /// <summary>
        /// Интервал в течении которого курьер заберет товар у продавца
        /// </summary>
        public DeliveryInterval FromSeller { get; set; }
        public string SellerDeliveryTrackingUrl { get; set; }
        /// <summary>
        /// Интервал в течении которого курьер привезет товар покупателю
        /// </summary>
        public DeliveryInterval ToBuyer { get; set; }
        public string BuyerDeliveryTrackingUrl { get; set; }

        public DeliveryOrderResponseModel()
        {
            
        }
        public DeliveryOrderResponseModel(DeliveryOrderResponseDto dto)
        {
            DeliveryPrice = dto.DeliveryPrice.Value;
            FromSeller = dto.FromSeller;
            SellerDeliveryTrackingUrl = dto.SellerDeliveryTrackingUrl;
            ToBuyer = dto.ToBuyer;
            BuyerDeliveryTrackingUrl = dto.BuyerDeliveryTrackingUrl;
        }

        public DeliveryOrderResponseModel Clone() => new DeliveryOrderResponseModel
        {
            DeliveryPrice = this.DeliveryPrice,
            FromSeller = this.FromSeller?.Clone(),
            ToBuyer = this.ToBuyer?.Clone(),
            BuyerDeliveryTrackingUrl = this.BuyerDeliveryTrackingUrl,
            SellerDeliveryTrackingUrl = this.SellerDeliveryTrackingUrl,
        };

        public static DeliveryOrderResponseModel ToDeliveryOrderResponseModel(DostavistaCalculateOrderResponse response)
        {
            if (response?.Order == null) return null;
            var order = response.Order;
            return new DeliveryOrderResponseModel
            {
                DeliveryPrice = decimal.Parse(order.payment_amount),
                FromSeller = new DeliveryInterval
                {
                    RequiredStartDatetime = order.points[0].required_start_datetime,
                    RequiredFinishDatetime = order.points[0].required_finish_datetime
                },
                ToBuyer = new DeliveryInterval
                {
                    RequiredStartDatetime = order.points[1].required_start_datetime,
                    RequiredFinishDatetime = order.points[1].required_finish_datetime
                },
                SellerDeliveryTrackingUrl = order.points[0].tracking_url?.ToString(),
                BuyerDeliveryTrackingUrl = order.points[1].tracking_url?.ToString()
            };
        }
    }
}


//required_start_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (от).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_start_datetime не должен быть задан, а на второй точке required_start_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов

//required_finish_datetime timestamp / null
//Ожидаемое время прибытия курьера на адрес (до).

//Значение по умолчанию: null.

//Для заказов типа same_day
//Для заказов типа same_day должно быть задано ровно 2 точки. Причем, на первой точке required_finish_datetime не должен быть задан, а на второй точке required_finish_datetime должен соответствовать диапозону, который можно получить из метода получения списка интервалов
