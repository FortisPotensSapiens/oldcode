using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbMerchServers")]
    [Index(nameof(MerchId), nameof(ServerId), IsUnique = true)]
    public class AbMerchServerDbDto : EntityDbDtoBase
    {
        public AbMerchDbDto Merch { get; set; }
        public Guid MerchId { get; set; }
        public AbServerDbDto Server { get; set; }
        public Guid ServerId { get; set; }
        public uint Qty { get; set; }
    }

    [Table("AbMerchServerExtentions")]
    [Index(nameof(MerchId), nameof(ServerExtentionId), IsUnique = true)]
    public class AbMerchServerExtentionsDbDto : EntityDbDtoBase
    {
        public AbMerchDbDto Merch { get; set; }
        public Guid MerchId { get; set; }
        public AbServerExtentionDto ServerExtention { get; set; }
        public Guid ServerExtentionId { get; set; }
        public uint Qty { get; set; }
    }
}
