using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record FloorNumber : ShortText
    {
        public FloorNumber(string value) : base(value)
        {
        }
        public static implicit operator string(FloorNumber value) => value is null ? null : value.Value;
        public static implicit operator FloorNumber(string value) => string.IsNullOrWhiteSpace(value) ? null : new FloorNumber(value);
    }

}

