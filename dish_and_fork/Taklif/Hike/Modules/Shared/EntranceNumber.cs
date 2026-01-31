using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record EntranceNumber : ShortText
    {
        public EntranceNumber(string value) : base(value)
        {
        }
        public static implicit operator string(EntranceNumber value) => value is null ? null : value.Value;
        public static implicit operator EntranceNumber(string value) => string.IsNullOrWhiteSpace(value) ? null : new EntranceNumber(value);
    }

}

