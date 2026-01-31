using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record PageSize : NotDefault<int>
    {
        public PageSize(int value) : base(value)
        {
            if (value < 0)
                throw new ApplicationException("Номер страницы должен быть больше нуля!");
            if (value > 10000)
                throw new ApplicationException("Размер страницы страницы слишком большой!");
        }
        public static implicit operator int(PageSize value) => value is null ? default : value.Value;
        public static implicit operator PageSize(int value) => value == default ? null : new PageSize(value);
        public static implicit operator uint(PageSize value) => value is null ? default : (uint)value.Value;
        public static implicit operator PageSize(uint value) => value == default ? null : new PageSize((int)value);
    }

}

