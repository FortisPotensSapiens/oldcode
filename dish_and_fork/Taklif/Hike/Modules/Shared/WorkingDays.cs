using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record WorkingDays : NotEmptyCollection<DayOfWeek>
    {
        public WorkingDays(ICollection<DayOfWeek> values) : base(values)
        {
            if (values.Count < 1)
                throw new ApplicationException("Количество рабочих больше < 1");
            if (values.Count > 7)
                throw new ApplicationException("Количество рабочих дней больше 7");
        }
        public static implicit operator List<DayOfWeek>(WorkingDays value) => value is null ? new() : value.Value.ToList();
        public static implicit operator WorkingDays(List<DayOfWeek> value) => value is null || value.Count == 0 ? null : new WorkingDays((ICollection<DayOfWeek>)value);
        public static implicit operator DayOfWeek[](WorkingDays value) => value is null ? new DayOfWeek[0] : value.Value.ToArray();
        public static implicit operator WorkingDays(DayOfWeek[] value) => value is null || value.Length == 0 ? null : new WorkingDays((ICollection<DayOfWeek>)value);
    }

}

