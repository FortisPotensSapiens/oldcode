using AleBotApi.DbDtos.Enums;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbAccountTransactions")]
    [Index(nameof(ExternalId))]
    public class AbAccountTransactionDbDto : EntityDbDtoBase
    {
        public AbAccountDbDto Account { get; set; }
        public Guid AccountId { get; set; }
        public AbPaymentSystemDbDto PaymentSystem { get; set; }
        public Guid? PaymentSystemId { get; set; }
        public AbPaymentNetworkDbDto PaymentNetwork { get; set; }
        public Guid? PaymentNetworkId { get; set; }

        [Column(TypeName = "character varying(8)"), MaxLength(8)]
        public AccountTransactionOperationType OperationType { get; set; }

        [Column(TypeName = "character varying(24)"), MaxLength(8)]
        public AccountTransactionReason Reason { get; set; }

        public decimal Amount { get; set; }
        [MaxLength(100)]
        public string? DebitCryptoWalletAddress { get; set; }

        [Column(TypeName = "character varying(9)"), MaxLength(9)]
        public AccountTransactionState State { get; set; }

        public DateTime? CompletedOn { get; set; }

        public DateTime? CancelledOn { get; set; }

        /// <summary>
        /// Id во внейшней системе. На данный момент InvoceId в CryptoCloud
        /// </summary>
        [MaxLength(100)]
        public string? ExternalId { get; set; }

        public AbCurrencyDbDto? DebitCurrency { get; set; }

        /// <summary>
        /// Валюта в которой хочет получить сумму пользователь. Может отличаться от валиты в которой мы отдаем. 
        /// Например мы отдаем с рублевого счета рубли на его доллоровый счет приходят доллары. Тогда тут будет записан доллар.
        /// </summary>
        public Guid? DebitCurrencyId { get; set; }

        /// <summary>
        /// Комиссия сети или за перевод. Комиссия за платеж. Сколько денег ушло на комиссию.
        /// </summary>
        public decimal? DebitFee { get; set; }

        /// <summary>
        /// Описание операции
        /// </summary>
        public string? OperationDescription { get; set; }
        public AbUserDbDto? ByReferal { get; set; }
        /// <summary>
        /// Если это платеж за реферала то Id реферала за которого получили деньги
        /// </summary>
        public Guid? ByReferalId { get; set; }
    }
}
