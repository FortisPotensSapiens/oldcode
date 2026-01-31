using System.Threading.Tasks;
using Daf.DeliveryModule.Domain;

namespace Daf.DeliveryModule.SecondaryAdaptersInterfaces
{
    public interface IDostavistaClient
    {
        Task<DostavistaCalculateOrderResponse> CalculateOrder(DostavistaCalculateOrderRequest request);
        Task<DostavistaCalculateOrderResponse> CreateOrder(DostavistaCalculateOrderRequest request);
        Task<DostavistaCancelOrderResponse> CancelOrder(uint orderId);
        Task<DostavistaOrder> GetOrder(uint orderId);
        Task<DostavistaFindOrdersResponse> GetOrder(DostavistaFindOrdersRequest request);
        Task<DostavistaGetIntervalsResponse> GetDeliveryIntervals(DateTime? date = null);
    }
}
