using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record DeliveryInfo(
        Money DeliveryPrice,
        PeriodEntity FromSeller,
        NotNullOrWhitespaceString SellerDeliveryTrackingUrl,
        PeriodEntity ToBuyer,
        NotNullOrWhitespaceString BuyerDeliveryTrackingUrl
        );

}

