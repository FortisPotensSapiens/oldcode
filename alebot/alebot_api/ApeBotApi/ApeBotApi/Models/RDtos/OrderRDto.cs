using AleBotApi.DbDtos;
using AleBotApi.DbDtos.Enums;

namespace AleBotApi.Models.RDtos
{
    public class OrderRDto
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public string? UserEmail { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid PaymentNetworkId { get; set; }

        public string? TradingAccount { get; set; }

        public DateTime? PurchasedOn { get; set; }

        public OrderState State { get; set; }

        public decimal Amount { get; set; }

        public string? ExternalId { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<OrderLineRDto> Lines { get; set; } = null!;
    }

    public class OrderLineRDto
    {
        public Guid OrderLineId { get; set; }

        public Guid MerchId { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public Guid CurrencyId { get; set; }
    }
}
