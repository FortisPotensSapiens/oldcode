
using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record GreaterThanZero<T> : ValueObject<T> where T : IComparable<T>
    {
        public GreaterThanZero(T value) : base(value)
        {
            if (value.CompareTo(default) <= 0)
                new { value }.ThrowApplicationException("Value <= 0");
        }
    }
}
