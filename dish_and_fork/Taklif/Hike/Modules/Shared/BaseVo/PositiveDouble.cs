

using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record PositiveDouble : NotDefault<double>
    {
        public PositiveDouble(double value) : base(value)
        {
            if (value < 0)
                new { Vaelue = value }.ThrowApplicationException("Вес не может быть меньше нуля!");
        }
        public static implicit operator double(PositiveDouble value) => value is null ? default : value.Value;
        public static implicit operator PositiveDouble(double value) => value == default ? null : new PositiveDouble(value);
    }

}

