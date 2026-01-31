using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record IntercomCode : ShortText
    {
        public IntercomCode(string value) : base(value)
        {
        }
        public static implicit operator string(IntercomCode value) => value is null ? null : value.Value;
        public static implicit operator IntercomCode(string value) => string.IsNullOrWhiteSpace(value) ? null : new IntercomCode(value);
    }

}

