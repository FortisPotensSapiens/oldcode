using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record ApartmentNumber : ShortText
    {
        public ApartmentNumber(string value) : base(value)
        {
        }
        public static implicit operator string(ApartmentNumber value) => value is null ? null : value.Value;
        public static implicit operator ApartmentNumber(string value) => string.IsNullOrWhiteSpace(value) ? null : new ApartmentNumber(value);
    }

}

