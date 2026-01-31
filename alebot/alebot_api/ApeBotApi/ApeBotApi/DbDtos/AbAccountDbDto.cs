using ApeBotApi.DbDtos;
using Microsoft.AspNetCore.Http.HttpResults;
using MimeKit;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbAccounts")]
    public class AbAccountDbDto : EntityDbDtoBase
    {
        public AbUserDbDto User { get; set; } = null!;
        public Guid UserId { get; set; }
        public AbCurrencyDbDto Currency { get; set; } = null!;
        public Guid CurrencyId { get; set; }

        public decimal Amount { get; set; }
        public List<AbAccountTransactionDbDto> Transactions { get; set; } = new();
    }
}
