namespace Hike.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime time) => new DateTime(time.Ticks, DateTimeKind.Utc);
        public static TimeOnly ToTimeOnly(this DateTime time) => TimeOnly.FromDateTime(time.ToUtc());
        public static DateTime? ToUtc(this DateTime? time) => time.HasValue ? (DateTime?)new DateTime(time.Value.Ticks, DateTimeKind.Utc) : null;

    }
}
