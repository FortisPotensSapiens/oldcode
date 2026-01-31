using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record GreaterThanZeroDouble : ValueObject<double>
    {
        public GreaterThanZeroDouble(double value) : base(value)
        {
            if (!(value > 0))
                new { Vaelue = value }.ThrowApplicationException("Размер не может быть меньше нуля!");
        }
        public static implicit operator double(GreaterThanZeroDouble value) => value.Value;
        public static implicit operator GreaterThanZeroDouble(double value) => new GreaterThanZeroDouble(value);
    }

}

