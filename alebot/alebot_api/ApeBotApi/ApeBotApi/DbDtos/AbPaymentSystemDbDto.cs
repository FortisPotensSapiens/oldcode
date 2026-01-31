using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbPaymentSystems")]
    public class AbPaymentSystemDbDto : EntityDbDtoBase
    {
        [MaxLength(32)]
        public string Name { get; set; } = null!;
        public List<AbAccountTransactionDbDto> Transactions { get; set; } = new();
    }
}
