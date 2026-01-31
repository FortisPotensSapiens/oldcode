using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApeBotApi.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj) => obj is { } ? JsonSerializer.Serialize(obj, new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new JsonStringEnumConverter() }
        }) : string.Empty;

        public static void ThrowApplicationException(this object obj, string message)
        {
            throw new ApplicationException(message)
            {
                Data = { ["args"] = obj }
            };
        }
    }
}
