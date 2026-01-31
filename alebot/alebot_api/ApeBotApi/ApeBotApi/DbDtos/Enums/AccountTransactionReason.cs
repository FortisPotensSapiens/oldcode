namespace AleBotApi.DbDtos.Enums
{
    public enum AccountTransactionReason
    {
        /// <summary>
        /// Пополнение c CryptoCloud
        /// </summary>
        AccrualByCryptoCloud,

        /// <summary>
        /// Начисление за операции реферала
        /// </summary>
        AccrualByReferal,

        /// <summary>
        /// Вывод в CryptoCloud
        /// </summary>
        DebitingToCryptoCloud,
    }
}
