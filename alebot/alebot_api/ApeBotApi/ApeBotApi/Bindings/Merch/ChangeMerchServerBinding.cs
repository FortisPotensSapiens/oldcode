using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class ChangeMerchServerBinding
    {
        [Required]
        public Guid MerchServerId { get; set; }

        [Required]
        [Range(1, 100)]
        public uint Qty { get; set; }
    }
}
