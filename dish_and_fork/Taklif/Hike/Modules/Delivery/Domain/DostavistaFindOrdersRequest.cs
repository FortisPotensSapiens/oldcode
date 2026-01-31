namespace Daf.DeliveryModule.Domain
{
    public class DostavistaFindOrdersRequest
    {
        public List<int> OrderId { get; set; }
        public DostavistaOrderStatus? Status { get; set; }
        public int Offset { get; set; } = 0;
        public int Count { get; set; } = 50;
    }
}