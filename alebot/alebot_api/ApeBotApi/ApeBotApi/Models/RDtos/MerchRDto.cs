namespace AleBotApi.Models.RDtos
{
    public class MerchBriefRDto
    {
        public Guid Id { get; set; }

        public MerchCurrencyRDto Currency { get; set; } = null!;

        public string Name { get; set; } = null!;

        public byte[] Photo { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public decimal Price { get; set; }

        public IEnumerable<MerchCourseRDto> Courses { get; set; } = null!;

        public IEnumerable<MerchLicenseRDto> Licenses { get; set; } = null!;

        public IEnumerable<MerchServerRDto> Servers { get; set; } = null!;
    }

    public class MerchRDto : MerchBriefRDto
    {
        public string FullDescription { get; set; } = null!;
    }

    public class MerchCurrencyRDto
    {
        public Guid CurrencyId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
    }

    public class MerchCourseRDto
    {
        public Guid CourseId { get; set; }

        public string Name { get; set; } = null!;
    }

    public class MerchLicenseRDto
    {
        public Guid LicenseId { get; set; }

        public string Name { get; set; } = null!;

        public uint Qty { get; set; }
    }

    public class MerchServerRDto
    {
        public Guid ServerId { get; set; }

        public string Name { get; set; } = null!;

        public uint Qty { get; set; }
    }
}
