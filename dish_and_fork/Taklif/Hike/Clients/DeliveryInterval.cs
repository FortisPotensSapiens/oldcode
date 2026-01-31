namespace Hike.Clients
{
    public class DeliveryInterval
    {
        public DateTime RequiredStartDatetime { get; set; }
        public DateTime RequiredFinishDatetime { get; set; }
        public DeliveryInterval Clone() => new DeliveryInterval { RequiredFinishDatetime = RequiredFinishDatetime, RequiredStartDatetime= RequiredStartDatetime };
    }
}
