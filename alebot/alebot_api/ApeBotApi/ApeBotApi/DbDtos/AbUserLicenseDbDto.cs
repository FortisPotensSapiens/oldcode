using AleBotApi.Migrations;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbUserLicenses")]
    [Index(nameof(UserId), nameof(LicenseId), nameof(ActivationKey), IsUnique = true)]
    public class AbUserLicenseDbDto : EntityDbDtoBase
    {
        public AbUserDbDto User { get; set; }
        public Guid UserId { get; set; }
        public AbLicenseDbDto License { get; set; }
        public Guid LicenseId { get; set; }

        public string ActivationKey { get; set; } = null!;

        public string? TradingAccount { get; set; }
        public AbOrderLineDbDto? OrderLine { get; set; }
        public Guid? OrderLineId { get; set; }
    }
}
