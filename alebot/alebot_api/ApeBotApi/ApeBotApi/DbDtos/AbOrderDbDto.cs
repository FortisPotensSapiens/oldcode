using AleBotApi.DbDtos.Enums;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Index(nameof(ExternalId))]
    [Table("AbOrders")]
    public class AbOrderDbDto : EntityDbDtoBase
    {
        public AbUserDbDto User { get; set; }
        public Guid UserId { get; set; }
        public AbCurrencyDbDto Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public AbPaymentNetworkDbDto PaymentNetwork { get; set; }
        public Guid PaymentNetworkId { get; set; }

        [StringLength(20, MinimumLength = 4)]
        public string? TradingAccount { get; set; }

        public DateTime? PurchasedOn { get; set; }


        [Column(TypeName = "character varying(9)"), MaxLength(9)]
        public OrderState State { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Id во внейшней системе. На данный момент InvoceId в CryptoCloud
        /// </summary>
        [MaxLength(100)]
        public string? ExternalId { get; set; }
        public List<AbOrderLineDbDto> OrderLines { get; set; } = new();
    }
}
