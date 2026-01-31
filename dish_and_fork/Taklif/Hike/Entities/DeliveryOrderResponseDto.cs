

using Hike.Clients;

namespace Hike.Entities
{
    public class DeliveryOrderResponseDto
    {
        /// <summary>
        /// Цена доставки
        /// </summary>
        public MoneyDto DeliveryPrice { get; set; }
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

        //public DeliveryInfo ToDeliveryInfo() =>
        //    new DeliveryInfo(new Rub(DeliveryPrice), FromSeller.ToPerion(), SellerDeliveryTrackingUrl, ToBuyer.ToPerion(), BuyerDeliveryTrackingUrl);

        //public DeliveryOrderResponseDto(DeliveryInfo entity)
        //{
        //    DeliveryPrice = new MoneyDto { CurrencyType = entity.DeliveryPrice.ToCurrency(), Value = entity.DeliveryPrice.Value };
        //    FromSeller = new DeliveryInterval { RequiredFinishDatetime = entity.FromSeller.End, RequiredStartDatetime = entity.FromSeller.Start };
        //    ToBuyer = new DeliveryInterval { RequiredFinishDatetime = entity.ToBuyer.End, RequiredStartDatetime = entity.ToBuyer.Start };
        //    SellerDeliveryTrackingUrl = entity.SellerDeliveryTrackingUrl;
        //    BuyerDeliveryTrackingUrl = entity.BuyerDeliveryTrackingUrl;
        //}

        //public DeliveryOrderResponseDto()
        //{

        //}
        public DeliveryOrderResponseDto Clone() => new DeliveryOrderResponseDto
        {
            DeliveryPrice = DeliveryPrice,
            FromSeller = FromSeller?.Clone(),
            ToBuyer = ToBuyer?.Clone(),
            BuyerDeliveryTrackingUrl = BuyerDeliveryTrackingUrl,
            SellerDeliveryTrackingUrl = SellerDeliveryTrackingUrl,
        };

        public static DeliveryOrderResponseDto ToDeliveryOrderResponseModel(DostavistaCalculateOrderResponse response)
        {
            if (response?.Order == null) return null;
            var order = response.Order;
            return new DeliveryOrderResponseDto
            {
                DeliveryPrice = new MoneyDto { Value = decimal.Parse(order.payment_amount), CurrencyType = CurrencyType.Rub },
                FromSeller = new DeliveryInterval
                {
                    RequiredStartDatetime = order.points[0].required_start_datetime ,
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
