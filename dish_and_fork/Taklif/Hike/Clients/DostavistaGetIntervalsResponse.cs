namespace Hike.Clients
{
    public class DostavistaGetIntervalsResponse
    {
        public List<DeliveryInterval> DeliveryIntervals { get; set; } = new();
        public DeliveryInterval GetEarliest() => DeliveryIntervals?.OrderBy(x => x.RequiredStartDatetime).ThenBy(x => x.RequiredFinishDatetime).FirstOrDefault();
    }
}
