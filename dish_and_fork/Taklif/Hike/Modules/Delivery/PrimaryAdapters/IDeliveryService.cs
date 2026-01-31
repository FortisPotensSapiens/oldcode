using System.Threading.Tasks;
using Daf.DeliveryModule.Domain;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.DeliveryModule.PrimaryAdapters
{
    public interface IDeliveryService
    {
        Task<List<DeliveryInterval>> GetDeliveryIntervals(NotDefaultDateTime? date = null);
        Task<(Money price, DateTime startTime, DateTime finishTime)> CalculateDeliveryPrice(
            NotDefaultDateTime? deliveryDate,
            Kilograms weight,
            StreetName sellerStreet,
            HouseNumber sellerhouseNumber,
            StreetName buyerStreet,
        HouseNumber buyerHouseNumber
            );

        Task<DeliveryInfo> Delivery(
            Address from,
            Address to,
            NotDefaultDateTime? deliveryDate,
            double weight,
            IEnumerable<(string wareCode, string description, int itemsCount, string itemPaymentAmount)> items,
            string clientOrderId
            );

        bool IsOrderChangedEventSignatureValid(string signature, string txt);
    }
}
