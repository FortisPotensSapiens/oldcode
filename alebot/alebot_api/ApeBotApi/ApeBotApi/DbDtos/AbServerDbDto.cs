using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbServers")]
    public class AbServerDbDto : EntityDbDtoBase
    {
        public string Name { get; set; } = null!;
        public List<AbUserServerDbDto> UserServers { get; set; } = new();
        public List<AbMerchServerDbDto> MerchServers { get; set; } = new();
        public uint ServerDurationInMonth { get; set; }
    }

    [Table("AbServersExtention")]
    public class AbServerExtentionDto : EntityDbDtoBase
    {
        public List<AbMerchServerExtentionsDbDto> MerchServerExtentions { get; set; } = new();
        public uint ServerDurationInMonth { get; set; }
    }
}
