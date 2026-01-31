namespace Daf.SharedModule.Domain
{
    public record Longitude : ValueObject<double>
    {
        public Longitude(double value) : base(value)
        {
        }

        public static implicit operator double(Longitude value) =>value is null ? default : value.Value;
        public static implicit operator Longitude(double value) => value == default ? null : new Longitude(value);
    }

}

