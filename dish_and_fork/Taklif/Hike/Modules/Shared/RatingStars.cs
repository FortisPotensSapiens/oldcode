namespace Daf.SharedModule.Domain
{
    public record RatingStars : ClosedRange<byte>
    {
        public RatingStars(byte value) : base(value, 1, 5)
        {
        }

        public static implicit operator int(RatingStars value) => value is null ? default : value.Value;
        public static implicit operator RatingStars(int value) => value == default ? null : new RatingStars((byte)value);
        public static implicit operator double(RatingStars value) => value is null ? default : value.Value;
        public static implicit operator RatingStars(double value) => value == default ? null : new RatingStars((byte)value);
        public static implicit operator byte(RatingStars value) => value is null ? default : value.Value;
        public static implicit operator RatingStars(byte value) => value == default ? null : new RatingStars(value);
    }

}

