using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record StreetName : ShortText
    {
        public StreetName(string value) : base(value)
        {
        }
        public static implicit operator string(StreetName value) => value is null ? null : value.Value;
        public static implicit operator StreetName(string value) => string.IsNullOrWhiteSpace(value) ? null : new StreetName(value);
    }

}

