namespace Daf.SharedModule.Domain
{
    public abstract record ClosedRange<T> : ValueObject<T> where T : IComparable<T>
    {
        public ClosedRange(T value, T from, T to) : base(value)
        {
            if (to.CompareTo(from) < 0)
                throw new ApplicationException("To < From")
                { Data = { ["args"] = new { Value, from, to } } };
            if (value.CompareTo(from) < 0)
                throw new ApplicationException("Value < From")
                { Data = { ["args"] = new { Value, from, to } } };
            if (value.CompareTo(to) > 0)
                throw new ApplicationException("Value > To")
                { Data = { ["args"] = new { Value, from, to } } };
        }
    }
}
