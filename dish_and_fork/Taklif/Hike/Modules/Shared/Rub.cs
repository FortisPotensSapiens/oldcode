using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record Rub : Money
    {
        public Rub(decimal value) : base(value)
        {
        }

        public static implicit operator decimal(Rub value) => value is null ? default : value.Value;
        public static implicit operator Rub(decimal value) => value == default ? null : new Rub(value);
        public static Rub? From(decimal? value) => value.HasValue ? new Rub(value.Value) : null;

        public override Money Add(Money money)
        {
            if (money is not Rub)
                new { money }.ThrowApplicationException("Нельзя добавлять валюту другого типа");
            return new Rub(money + Value);
        }

        public override Money Sub(Money money)
        {
            if (money is not Rub)
                new { money }.ThrowApplicationException("Нельзя вычитать валюту другого типа");
            return new Rub(Value - money);
        }

        public override Money Mul(ValueObject<decimal> value)
        {
            return new Rub(Value * value.Value);
        }

        public override Money Div(NotDefault<decimal> value)
        {
            return new Rub(Value / value.Value);
        }
    }

}

