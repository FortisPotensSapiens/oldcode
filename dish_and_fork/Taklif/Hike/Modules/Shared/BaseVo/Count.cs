
using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record Count : ValueObject<int>
    {
        public Count(int value) : base(value)
        {
            if (value < 0)
                new { Value = value }.ThrowApplicationException("Количество меньше нуля");
        }

        public static implicit operator int(Count value) => value is null ? default : value.Value;
        public static implicit operator Count(int value) => value == default ? null : new Count(value);
        public static implicit operator uint(Count value) => value is null ? default : (uint)value.Value;
        public static implicit operator Count(uint value) => value == default ? null : new Count((int)value);
    }

}

