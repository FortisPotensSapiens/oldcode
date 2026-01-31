using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Wallet
{
    public class CreateAccountTransactionDebitingBinding
    {
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        [Range(10, 50000)]
        public decimal Amount { get; set; }
        [Required]
        public Guid PaymentNetworkId { get; set; }
        [Required]
        public Guid PaymentCurrencyId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string DebitCryptoWalletAddress { get; set; } = null!;
    }
}
