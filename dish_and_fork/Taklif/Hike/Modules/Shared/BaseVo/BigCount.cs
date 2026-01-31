

using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record BigCount : ValueObject<long>
    {
        public BigCount(long value) : base(value)
        {
            if (value < 0)
                new { Value = value }.ThrowApplicationException("Количество меньше нуля");
        }

        public static implicit operator long(BigCount value) => value is null ? default : value.Value;
        public static implicit operator BigCount(long value) => value == default ? null : new BigCount(value);
    }

}

