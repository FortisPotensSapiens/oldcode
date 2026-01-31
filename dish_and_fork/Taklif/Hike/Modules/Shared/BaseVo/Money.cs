namespace Daf.SharedModule.Domain.BaseVo
{
    public abstract record Money : NotDefault<decimal>
    {
        public Money(decimal value) : base(value)
        {
            if (value < 0)
                throw new ApplicationException("Цена меньше нуля!");
        }

        public abstract Money Add(Money money);
        public abstract Money Sub(Money money);
        public abstract Money Mul(ValueObject<decimal> value);
        public abstract Money Div(NotDefault<decimal> value);
        public string Format() => Value.ToString("##.##");
    }

}

