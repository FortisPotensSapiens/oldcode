

namespace ApeBotApi.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinStrings(this IEnumerable<string> strings, string separator = "\n")
            => string.Join(separator, strings);

        //public static NotEmptyCollection<T> ToNotEmpty<T>(this IEnumerable<T> value)
        //{
        //    if (value == null)
        //        return null;
        //    var values = value.ToList();
        //    if (values.Count == 0)
        //        return null;
        //    return new NotEmptyCollection<T>((ICollection<T>)values);
        //}
    }
}
