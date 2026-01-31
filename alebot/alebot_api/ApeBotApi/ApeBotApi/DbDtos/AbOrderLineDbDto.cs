using AleBotApi.Migrations;
using ApeBotApi.DbDtos;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbOrderLines")]
    [Index(nameof(MerchId), nameof(OrderId), IsUnique = true)]
    public class AbOrderLineDbDto : EntityDbDtoBase
    {
        public AbOrderDbDto Order { get; set; }
        public Guid OrderId { get; set; }
        public AbMerchDbDto Merch { get; set; }
        public Guid MerchId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
        public AbCurrencyDbDto Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public List<AbUserCourseDbDto> UserCourses { get; set; } = new();
        public List<AbUserLicenseDbDto> UserLicenses { get; set; } = new();
        public List<AbUserServerDbDto> UserServers { get; set; } = new();
        public AbUserServerDbDto? ExtendedUserServer { get; set; } = null!;
        public Guid? ExtendedUserServerId { get; set; }

    }
}
