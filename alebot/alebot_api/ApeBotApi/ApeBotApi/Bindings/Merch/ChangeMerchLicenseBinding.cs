using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class ChangeMerchLicenseBinding
    {
        [Required]
        public Guid MerchLicenseId { get; set; }

        [Required]
        [Range(1, 100)]
        public uint Qty { get; set; }
    }
}
