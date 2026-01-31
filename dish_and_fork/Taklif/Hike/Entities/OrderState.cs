

namespace Hike.Entities
{
    public enum OrderState
    {
        Created = 10,
        Paid = 11,
        Delivering = 13,
        Delivered = 12
    }
    //public static class OrderStateExtentions
    //{
    //    public static OrderState ToOrderState(this Daf.OrderingModule.Domain.OrderStateVo state) =>
    //        state switch
    //        {
    //            DeliveredOrder => OrderState.Delivered,
    //            DeliveringOrder => OrderState.Delivering,
    //            PaidOrder => OrderState.Paid,
    //            CreatedOder => OrderState.Created,
    //            _ => throw new NotImplementedException()
    //        };
    //    public static Daf.OrderingModule.Domain.OrderStateVo ToOrderState(
    //        this OrderState state,
    //        NotDefaultDateTime? paymentDate = null,
    //        DeliveryInfo? deliveryInfo = null,
    //        NotDefaultDateTime? deliveredDate = null
    //        ) =>
    //        state switch
    //        {
    //            OrderState.Created => new CreatedOder(),
    //            OrderState.Paid => new PaidOrder(paymentDate),
    //            OrderState.Delivering => new DeliveringOrder(paymentDate, deliveryInfo),
    //            OrderState.Delivered => new DeliveredOrder(paymentDate, deliveryInfo, deliveredDate)
    //        };

    //}
}
