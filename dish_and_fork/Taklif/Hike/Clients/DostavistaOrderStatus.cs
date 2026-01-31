namespace Hike.Clients
{
    public enum DostavistaOrderStatus
    {
        /// <summary>
        /// Созданный заказ, ожидает одобрения оператора.
        /// </summary>
        New = 10,
        /// <summary>
        /// Заказ одобрен оператором и доступен курьерам.
        /// </summary>
        Available,
        /// <summary>
        /// Заказ выполняется курьером.
        /// </summary>
        Active,
        /// <summary>
        /// Заказ выполнен
        /// </summary>
        Completed,
        /// <summary>
        /// Заказ повторно активирован и вновь доступен курьерам.
        /// </summary>
        Reactivated,
        /// <summary>
        /// Черновик заказа.
        /// </summary>
        Draft,
        /// <summary>
        /// Заказ отменен.
        /// </summary>
        Canceled,
        /// <summary>
        /// Заказа отложен.
        /// </summary>
        Delayed
    }
}