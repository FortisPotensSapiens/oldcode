namespace Hike.Extensions
{
    public static class TimeOnlyExtensions
    {
        public static DateTime ToDateTime(this TimeOnly time) => new DateTime(1970, 1, 1, time.Hour, time.Minute, time.Second, DateTimeKind.Utc);
    }
}
