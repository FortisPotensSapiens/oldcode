using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record PageNumber : NotDefault<int>
    {
        public PageNumber(int value) : base(value)
        {
            if (value < 0)
                throw new ApplicationException("Номер страницы должен быть больше нуля!");
        }
        public static implicit operator int(PageNumber value) => value is null ? default : value.Value;
        public static implicit operator PageNumber(int value) => value == default ? null : new PageNumber(value);
        public static implicit operator uint(PageNumber value) => value is null ? default : (uint)value.Value;
        public static implicit operator PageNumber(uint value) => value == default ? null : new PageNumber((int)value);
    }

}

