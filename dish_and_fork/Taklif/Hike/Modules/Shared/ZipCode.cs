using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record ZipCode : ShortText
    {
        public ZipCode(string value) : base(value)
        {
        }
        public static implicit operator string(ZipCode value) => value is null ? null : value.Value;
        public static implicit operator ZipCode(string value) => string.IsNullOrWhiteSpace(value) ? null : new ZipCode(value);
    }
}

