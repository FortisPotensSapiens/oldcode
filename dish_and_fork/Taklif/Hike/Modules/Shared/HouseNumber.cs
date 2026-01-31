using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record HouseNumber : ShortText
    {
        public HouseNumber(string value) : base(value)
        {

        }
        public static implicit operator string(HouseNumber value) => value is null ? null : value.Value;
        public static implicit operator HouseNumber(string value) => string.IsNullOrWhiteSpace(value) ? null : new HouseNumber(value);
    }

}

