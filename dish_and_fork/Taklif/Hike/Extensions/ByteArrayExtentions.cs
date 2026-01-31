namespace Hike.Extensions
{
    public static class ByteArrayExtentions
    {
        public static string ToHexString(this byte[] bytes) => Convert.ToHexString(bytes);
    }
}
