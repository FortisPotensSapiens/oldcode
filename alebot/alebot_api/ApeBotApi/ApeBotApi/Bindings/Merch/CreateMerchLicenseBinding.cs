using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class CreateMerchLicenseBinding
    {
        [Required]
        public Guid MerchId { get; set; }

        [Required]
        public Guid LicenseId { get; set; }

        [Required]
        [Range(1, 100)]
        public uint Qty { get; set; }
    }
}
