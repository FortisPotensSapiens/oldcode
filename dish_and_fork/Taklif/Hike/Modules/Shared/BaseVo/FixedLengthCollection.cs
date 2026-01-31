namespace Daf.SharedModule.Domain.BaseVo
{
    public record FixedLengthCollection<T> : NotEmptyCollection<T>
    {
        public FixedLengthCollection(ICollection<T> values, int min, int max) : base(values)
        {
            if (values.Count < min)
                throw new ApplicationException("Строка слишком короткая!");
            if (values.Count > max)
                throw new ApplicationException("Строка слишком длинная!");
        }
    }
}
