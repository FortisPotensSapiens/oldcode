using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class CreateMerchServerBinding
    {
        [Required]
        public Guid MerchId { get; set; }

        [Required]
        public Guid ServerId { get; set; }

        [Required]
        [Range(1, 100)]
        public uint Qty { get; set; }
    }
}
