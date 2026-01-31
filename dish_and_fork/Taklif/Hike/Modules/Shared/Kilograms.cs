using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record Kilograms : Size
    {
        public Kilograms(double value) : base(value)
        {
        }

        public override Size Sub(Size value)
        {
            if (value is not Kilograms)
                new { value, old = this }.ThrowApplicationException("Вычитать можно из размеров одного типа");
            return new Kilograms(Value - value.Value);
        }

        public override Size Mul(double value)
            => new Kilograms(Value * value);

        public override Size Sub(double value) => new Kilograms(Value - value);

        public static implicit operator double(Kilograms value) => value is null ? default : value.Value;
        public static implicit operator Kilograms(double value) => value == default ? null : new Kilograms(value);
    }

}

