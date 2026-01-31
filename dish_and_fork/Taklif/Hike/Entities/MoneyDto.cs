
using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    [Owned]
    public class MoneyDto
    {

        [Range(0.0, 10000000.0)]
        public decimal Value { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public MoneyDto Clone() => new MoneyDto { Value = Value, CurrencyType = CurrencyType };

        //public Money ToMoney() =>
        //     CurrencyType.ToMoney(Value);

        //public MoneyDto(Money money)
        //{
        //    Value = money.Value;
        //    CurrencyType = money.ToCurrency();
        //}

        //public MoneyDto()
        //{

        //}
    }
}
