using Hike.Entities;
using Hike.Extensions;

namespace Hike.Models
{
    public class PeriodModel
    {
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }

        public Period ToPeriod() => new Period() { Start = Start.ToUtc(), End = End.ToUtc() };

        public Period DeepClone() => new Period() { Start = Start, End = End };
        public static PeriodModel From(Period dto)
        {
            if (dto == null)
                return null;
            return new PeriodModel()
            {
                End = dto.End,
                Start = dto.Start
            };
        }
    }
}
