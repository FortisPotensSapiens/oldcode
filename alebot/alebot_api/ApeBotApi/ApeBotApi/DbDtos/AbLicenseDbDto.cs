using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbLicenses")]
    public class AbLicenseDbDto : EntityDbDtoBase
    {
        public string Name { get; set; } = null!;
        public List<AbUserLicenseDbDto> UserLicense { get; set; } = new();
        public List<AbMerchLicenseDbDto> MerchLicenses { get; set; } = new();
    }
}
