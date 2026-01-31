using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApeBotApi.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<MemoryStream> ToMemoryStream(this Stream stream)
        {
            stream.Position = 0;
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms;
        }

        public static async Task<byte[]> ComputeHash(this Stream stream)
        {
            stream.Position = 0;
            using var md5 = MD5.Create();
            return await md5.ComputeHashAsync(stream);
        }
    }
}
