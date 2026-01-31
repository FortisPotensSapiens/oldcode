using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;

namespace ApeBotApi.Extensions
{

    public static class StringExtension
    {
        public static string GetNormalizedName(this string name) => string.IsNullOrWhiteSpace(name) ? null :
            string.Join(' ', name.Trim().ToUpper()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries));

        public static T ToObject<T>(
            this string json,
            JsonSerializerOptions options = default
            ) => string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(
            json,
            GetOptions(options)
            );

        private static JsonSerializerOptions GetOptions(JsonSerializerOptions options)
        {
            if (options == null)
                return new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    Converters = { new JsonStringEnumConverter() },
                };
            return options;
        }

        public static string GetExtention(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return ".file";
            var extension = Path.GetExtension(str);
            if (string.IsNullOrWhiteSpace(extension))
                return ".file";
            return extension;
        }

        public static string GetContentType(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "application/octet-stream";
            if (!new FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType))
                return "application/octet-stream";
            return contentType;
        }

        public static MemoryStream ToMemoryStream(this string str) => new MemoryStream(Encoding.UTF8.GetBytes(str));
    }
}
