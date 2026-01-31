using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record LongText : FixedLengthString
    {
        public LongText(string value) : base(value, 1, 1000)
        {
        }

        public static implicit operator string(LongText value) => value is null ? null : value.Value;
        public static implicit operator LongText(string value) => string.IsNullOrWhiteSpace(value) ? null : new LongText(value);
    }

}

