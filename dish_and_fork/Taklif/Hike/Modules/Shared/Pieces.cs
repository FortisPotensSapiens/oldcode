using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record Pieces : Size
    {
        public Pieces(int value) : base(value)
        {
        }

        public override Size Mul(double value)
       => new Pieces((int)(Value * value));

        public override Size Sub(double value) => new Pieces((int)(Value - value));

        public override Size Sub(Size value)
        {
            if (value is not Pieces)
                new { value, old = this }.ThrowApplicationException("Вычитать можно из размеров одного типа");
            return new Pieces((int)(Value - value.Value));
        }

        public static implicit operator int(Pieces value) => value is null ? default : (int)value.Value;
        public static implicit operator Pieces(int value) => value == default ? null : new Pieces(value);
    }

}

