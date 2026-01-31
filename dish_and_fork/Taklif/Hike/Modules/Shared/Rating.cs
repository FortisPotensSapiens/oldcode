using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record Rating : GreaterThanZeroDouble
    {
        public Rating(double value) : base(value)
        {
        }

        protected Rating(GreaterThanZeroDouble original) : base(original)
        {
        }

        public static Rating Calculate(IEnumerable<(UserId EvaluatorId, RatingStars Stars)> stars, IEnumerable<(GreaterThanZeroDouble Proportion, UserId UserId)> porportions)
        {
            var starsl = stars.ToList();
            var prop = porportions.ToList();
            double res = 0;
            foreach (var star in starsl)
            {
                var p = prop.FirstOrDefault(x => x.UserId == star.EvaluatorId);
                if (p.Proportion is { } && star.Stars is { })
                {
                    res += p.Proportion.Value * star.Stars.Value;
                }
            }
            return res;
        }

        public static implicit operator double(Rating value) => value is null ? default : value.Value;
        public static implicit operator Rating(double value) => value == default ? null : new Rating(value);
    }
}

