using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record PeriodEntity
    {
        public NotDefaultDateTime Start { get; init; }
        public NotDefaultDateTime End { get; init; }

        public PeriodEntity(NotDefaultDateTime start, NotDefaultDateTime end)
        {
            Start = start;
            End = end;
            Validate();
        }

        protected PeriodEntity(PeriodEntity other)
        {
            Start = other.Start;
            End = other.End;
            Validate();
        }

        public void Validate()
        {
            if (Start is null)
                throw new ApplicationException("Укажите начало периода");
            if (End is null)
                throw new ApplicationException("Укажите конец периода периода");
            if (End.Value < Start.Value)
                throw new ApplicationException("Конец периода меньше начала");
        }
    }

}

