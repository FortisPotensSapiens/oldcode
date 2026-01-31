using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record Liters : Size
    {
        public Liters(double value) : base(value)
        {
        }

        public override Size Sub(Size value)
        {
            if (value is not Liters)
                new { value, old = this }.ThrowApplicationException("Вычитать можно из размеров одного типа");
            return new Liters(Value - value.Value);
        }

        public override Size Mul(double value)
        => new Liters(Value * value);

        public override Size Sub(double value) => new Liters(Value - value);

        public static implicit operator double(Liters value) => value is null ? default : value.Value;
        public static implicit operator Liters(double value) => value == default ? null : new Liters(value);
    }

}

