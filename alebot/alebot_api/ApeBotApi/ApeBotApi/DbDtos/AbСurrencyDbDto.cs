using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace AleBotApi.DbDtos
{
    [Table("AbCurrencies")]
    public class AbCurrencyDbDto : EntityDbDtoBase
    {
        [MaxLength(4)]
        public string Code { get; set; } = null!;

        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public List<AbAccountDbDto> Accounts { get; set; } = new();
        public List<AbAccountTransactionDbDto> Transactions { get; set; } = new();
        public List<AbMerchDbDto> Merchs { get; set; } = new();
        public List<AbOrderDbDto> Orders { get; set; } = new();
        public List<AbOrderLineDbDto> OrderLines { get; set; } = new();
    }
}
