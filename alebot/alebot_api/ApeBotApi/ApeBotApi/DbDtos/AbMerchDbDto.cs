using ApeBotApi.DbDtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AleBotApi.DbDtos
{
    [Table("AbMerches")]
    public class AbMerchDbDto : EntityDbDtoBase
    {
        public AbCurrencyDbDto Currency { get; set; }
        public Guid CurrencyId { get; set; }
        public AbPaymentNetworkDbDto PaymentNetwork { get; set; }
        public Guid PaymentNetworkId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string ShortDescription { get; set; } = null!;

        public string FullDescription { get; set; } = null!;

        public byte[] Photo { get; set; } = null!;

        public decimal Price { get; set; }
        public List<AbMerchCourseDbDto> MerchCourses { get; set; } = new();
        public List<AbMerchLicenseDbDto> MerchLicenses { get; set; } = new();
        public List<AbMerchServerDbDto> MerchServers { get; set; } = new();
        public List<AbOrderLineDbDto> OrderLines { get; set; } = new();
        public List<AbMerchServerExtentionsDbDto> MerchServerExtentions { get; set; } = new();
    }
}
