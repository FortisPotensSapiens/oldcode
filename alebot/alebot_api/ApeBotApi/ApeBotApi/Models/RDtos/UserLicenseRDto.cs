namespace AleBotApi.Models.RDtos
{
    public class UserLicenseRDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string ActivationKey { get; set; } = null!;

        public string? TradingAccount { get; set; }
    }
}
