namespace Hike.Clients
{
    public class DostavistaPakage
    {
        /// <summary>
        /// Артикул товара.
        /// </summary>
        public string WareCode { get; set; }
        /// <summary>
        /// Описание товара. Максимум 1000 символов.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Количество товаров.
        /// </summary>
        public string ItemsCount { get; set; }
        /// <summary>
        /// Сумма оплаты за единицу товара.
        /// </summary>
        public string ItemPaymentAmount { get; set; }
        public string NomenclatureCode { get; set; }
    }
}
