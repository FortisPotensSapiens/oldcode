using AleBotApi.Migrations;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbMerchLicenses")]
    [Index(nameof(MerchId), nameof(LicenseId), IsUnique = true)]
    public class AbMerchLicenseDbDto : EntityDbDtoBase
    {
        public AbMerchDbDto Merch { get; set; }
        public Guid MerchId { get; set; }
        public AbLicenseDbDto License { get; set; }
        public Guid LicenseId { get; set; }

        public uint Qty { get; set; }
    }
}
