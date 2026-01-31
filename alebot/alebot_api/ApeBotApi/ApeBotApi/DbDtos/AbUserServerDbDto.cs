using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbUserServers")]
    [Index(nameof(UserId), nameof(Password), nameof(Address), nameof(Login), IsUnique = true)]
    public class AbUserServerDbDto : EntityDbDtoBase
    {
        public AbUserDbDto User { get; set; }
        public Guid UserId { get; set; }
        public AbServerDbDto Server { get; set; }
        public Guid ServerId { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;
        public DateTime? ExpirationDate { get; set; }
        public AbOrderLineDbDto? OrderLine { get; set; }
        public Guid? OrderLineId { get; set; }
        public string? ExternalId { get; set; }
        public List<AbOrderLineDbDto> OrderLinesToExtend { get; set; } = new();
    }
}
