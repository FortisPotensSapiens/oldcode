using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Merch
{
    public class CreateMerchBinding
    {
        [Required]
        public Guid CurrencyId { get; set; }
        [Required]
        public Guid PaymentNetworkId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string ShortDescription { get; set; } = null!;

        [Required]
        public string FullDescription { get; set; } = null!;

        [Required]
        public byte[] Photo { get; set; } = null!;

        [Required]
        [Range(0.01, 50000)]
        public decimal Price { get; set; }
    }
}
