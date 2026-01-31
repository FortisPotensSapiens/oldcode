namespace AleBotApi.Models.RDtos
{
    public class MyIncomeRDto
    {
        /// <summary>
        /// Всего. Весь доход.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Доход полученный с продуктов. Когда мои рефералы покупают продукты. Сейчас равен Total так как друго источника дохода пока нет.
        /// </summary>
        public decimal Products { get; set; }
    }
}
