using System.ComponentModel.DataAnnotations;

namespace AleBotApi.Bindings.Order
{
    public class CreateOrderBinding
    {
        [Required]
        public Guid MerchId { get; set; }

        public string? TradingAccount { get; set; }
        public Guid? UserServerId { get; set; }

        public Guid? UserId { get; set; }
    }
}
