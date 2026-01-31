using System.Threading.Tasks;

namespace Hike.Clients
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
