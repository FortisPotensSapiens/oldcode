namespace Daf.DeliveryModule.Domain
{
    public class DostavistaFindOrdersResponse : DostavistaResponseBase
    {
        public List<DostavistaOrder> Orders { get; set; }
    }
}