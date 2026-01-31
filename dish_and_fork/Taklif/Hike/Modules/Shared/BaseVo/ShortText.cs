namespace Daf.SharedModule.Domain.BaseVo
{
    public record ShortText : FixedLengthString
    {
        public ShortText(string value) : base(value, 1, 255)
        {
        }

        public static implicit operator string(ShortText value) => value is null ? null : value.Value;
        public static implicit operator ShortText(string value) => string.IsNullOrWhiteSpace(value) ? null : new ShortText(value);
    }

}

