namespace Hike.Clients
{
    public class DostavistaCalculateOrderRequest
    {
        /// <summary>
        /// Тип заказа
        /// </summary>
        public DostavistaOrderType Type { get; set; }
        /// <summary>
        /// Что везем
        /// </summary>
        public string Matter { get; set; }

        /// <summary>
        /// Общий вес отправления
        /// </summary>
        public uint TotalWeightKg { get; set; }

        public List<DostavistaPoint> Points { get; set; } = new List<DostavistaPoint>();
    }
}
