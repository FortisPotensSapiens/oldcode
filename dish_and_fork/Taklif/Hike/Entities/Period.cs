
using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    [Owned]
    public class Period
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        //public PeriodEntity ToPerion() => Start == default ? null : new PeriodEntity(Start, End);

        //public Period(PeriodEntity entity)
        //{
        //    Start = entity.Start;
        //    End = entity.End;
        //}

        //public Period()
        //{

        //}
    }
}
