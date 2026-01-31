using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Wallet
{
    public class CreateAccountTransactionAccrualBinding
    {
        [Required]
        public Guid AccountId { get; set; }

        [Required]
        [Range(10, 50000)]
        public decimal Amount { get; set; }

        [Required]
        public Guid PaymentSystemId { get; set; }
    }
}
