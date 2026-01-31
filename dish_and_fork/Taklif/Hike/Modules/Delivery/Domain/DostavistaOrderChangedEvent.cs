namespace Daf.DeliveryModule.Domain
{
    public class DostavistaOrderChangedEvent
    {
        public DateTime? event_datetime { get; set; }
        public DostavistaEventType? event_type { get; set; }
        public DostavistaOrder? order { get; set; }
    }
}
