namespace Daf.SharedModule.Domain
{
    public record Latitude : ValueObject<double>
    {
        public Latitude(double value) : base(value)
        {
        }

        public static implicit operator double(Latitude value) => value is null ? default : value.Value;
        public static implicit operator Latitude(double value) => value == default ? null : new Latitude(value);
    }

}

