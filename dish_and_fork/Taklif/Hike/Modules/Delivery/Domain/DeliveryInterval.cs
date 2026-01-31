using Daf.SharedModule.Domain;

namespace Daf.DeliveryModule.Domain
{
    public class DeliveryInterval
    {
        public DateTime RequiredStartDatetime { get; set; }
        public DateTime RequiredFinishDatetime { get; set; }
        public PeriodEntity ToPerion() => new PeriodEntity(RequiredStartDatetime, RequiredFinishDatetime);
        public DeliveryInterval Clone() => new DeliveryInterval { RequiredFinishDatetime = RequiredFinishDatetime, RequiredStartDatetime = RequiredStartDatetime };
    }
}
