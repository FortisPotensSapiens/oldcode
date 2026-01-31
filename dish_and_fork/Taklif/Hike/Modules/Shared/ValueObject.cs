namespace Daf.SharedModule.Domain
{
    public record ValueObject<T> : IValueObject
    {
        public T Value { get; }
        public ValueObject(T value)
        {
            Value = value;
        }

        public object? GetValue()
        {
            return Value;
        }

        public override string ToString()
        => Value.ToString();
    }

}

