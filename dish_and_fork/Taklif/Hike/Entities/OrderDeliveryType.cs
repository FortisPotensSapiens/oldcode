
namespace Hike.Entities
{
    public enum OrderDeliveryType
    {
        Now = 10,
        SelfDelivered
    }

    //public static class OrderDeliveryTypeExtention
    //{
    //    public static DeliveryType ToDeliveryType(this OrderDeliveryType type, Address address) => type switch
    //    {
    //        OrderDeliveryType.SelfDelivered => new SelfDelivered(),
    //        OrderDeliveryType.Now => new DeliveredByCouryer(address),
    //        _ => throw new NotImplementedException()
    //    };

    //    public static OrderDeliveryType ToDeliveryType(this DeliveryType type) => type switch
    //    {
    //        SelfDelivered sd => OrderDeliveryType.SelfDelivered,
    //        DeliveredByCouryer dbc => OrderDeliveryType.Now
    //    };
    //}
}
