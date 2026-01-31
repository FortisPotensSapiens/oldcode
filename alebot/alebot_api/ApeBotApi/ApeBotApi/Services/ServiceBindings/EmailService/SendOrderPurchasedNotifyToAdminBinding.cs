namespace AleBotApi.Services.ServiceBindings.EmailService
{
    public class SendOrderPurchasedNotifyToAdminBinding
    {
        public Guid UserId { get; set; }

        public string? UserEmail { get; set; }

        public Guid OrderId { get; set; }

        public decimal OrderAmount { get; set; }

        public List<SendOrderPurchasedNotifyToAdminBinding_OrderLine> OrderLines { get; set; } = null!;
    }

    public class SendOrderPurchasedNotifyToAdminBinding_OrderLine
    {
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
