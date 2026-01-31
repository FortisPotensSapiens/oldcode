namespace Hike.Clients
{
    public class DostavistaPoint
    {
        /// <summary>
        /// Полный адрес в формате: город, улица, дом. Максимум 350 символов.
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Данные контактного лица на точке.
        /// </summary>
        public DostavistaContact ContactPerson { get; set; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        public string ClientOrderId { get; set; }
        /// <summary>
        /// Дополнительная информация курьеру
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Номер здания
        /// </summary>
        public string BuildingNumber { get; set; }
        /// <summary>
        /// Подъезд
        /// </summary>
        public string EntranceNumber { get; set; }
        /// <summary>
        /// Код домофона
        /// </summary>
        public string IntercomCode { get; set; }
        /// <summary>
        /// Этаж
        /// </summary>
        public string FloorNumber { get; set; }
        /// <summary>
        /// Квартира или офис
        /// </summary>
        public string ApartmentNumber { get; set; }

        public List<DostavistaPakage> Packages { get; set; } = new List<DostavistaPakage>();
        public DateTime? RequiredStartDatetime { get; set; }
        public DateTime? RequiredFinishDatetime { get; set; }
    }
}
