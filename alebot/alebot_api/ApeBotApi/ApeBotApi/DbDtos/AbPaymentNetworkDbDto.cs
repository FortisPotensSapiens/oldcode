using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbPaymentNetworks")]
    public class AbPaymentNetworkDbDto : EntityDbDtoBase
    {
        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public List<AbAccountTransactionDbDto> Transactions { get; set; } = new();
        public List<AbMerchDbDto> Merchs { get; set; } = new();
        public List<AbOrderDbDto> Orders { get; set; } = new();
    }
}
