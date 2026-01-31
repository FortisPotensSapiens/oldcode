using ApeBotApi.DbDtos;

namespace AleBotApi.Models.RDtos
{
    public class MyReferralsRDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int FirstLine { get; set; }
        public int SecondLine { get; set; }

        public List<MyReferralsReferralRDto> Referrals { get; set; } = new();
    }

    public sealed class MyReferralsReferralRDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public decimal MyIncome { get; set; }
        public int Total { get; set; }
        public int Active { get; set; }
        public int ProductCount { get; set; }
    }

    public class ReferrerRDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public IEnumerable<AbUserDbDto> Level1 { get; set; } = null!;
        public IEnumerable<AbUserDbDto> Level2 { get; set; } = null!;
        public IEnumerable<AbUserDbDto> Level3 { get; set; } = null!;
        public IEnumerable<Guid> MerchIds { get; set; } = null!;
        public IEnumerable<decimal> AccrualByReferalTransactions { get; set; } = null!;
        public int Total => Level1.Count() + Level2.Count() + Level3.Count();
        public int Level1Active => Level1.Where(x => x.LastActiveTime > DateTime.UtcNow.AddMonths(-1)).Count();
        public int Active => Level1.Where(x => x.LastActiveTime > DateTime.UtcNow.AddMonths(-1)).Count()
            + Level2.Where(x => x.LastActiveTime > DateTime.UtcNow.AddMonths(-1)).Count()
            + Level3.Where(x => x.LastActiveTime > DateTime.UtcNow.AddMonths(-1)).Count();
        public decimal MyIncome => AccrualByReferalTransactions.Sum();
        public int ProductCount => MerchIds.Count();
    }
}
