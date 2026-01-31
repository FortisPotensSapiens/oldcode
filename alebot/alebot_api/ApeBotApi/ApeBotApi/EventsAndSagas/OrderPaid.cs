namespace AleBotApi.EventsAndSagas
{
    public class OrderPaid
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public bool IsSendOrderPurchasedNotifyToAdmin { get; set; }
        public bool IsSendOrderPurchasedNotifyUser { get; set; }
        public int SendOrderPurchasedNotifyToAdminRetryCount { get; set; }
        public int SendOrderPurchasedNotifyUserCount { get; set; }
        public List<Guid> HandledMerchesIds { get; set; } = new();
        public List<Guid> HandledMerchServerIds { get; set; } = new();
        public List<Guid> HandledMerchServerExtentionIds { get; set; } = new();
    }
}
