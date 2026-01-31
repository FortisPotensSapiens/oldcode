using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Hike.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> PostAsync<TResponse, TRequest>(this HttpClient client, Uri uri, TRequest request, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
        {
            var response = await client.PostAsJsonAsync(uri, request, options, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Не удалось выполнить запрос")
                {
                    Data =
                    {
                        ["response"] = response.ToString(),
                        ["body"] = body
                    }
                };
            return body.ToObject<TResponse>(options);
        }

        public static async Task<TResponse> GetJsonAsync<TResponse>(this HttpClient client, Uri uri, IEnumerable<(string, object)> parameters = default, JsonSerializerOptions options = default, CancellationToken cancellationToken = default)
        {
            if (parameters != null)
            {
                var pstr = parameters.Where(x => !string.IsNullOrWhiteSpace(x.Item2?.ToString())).Select(x =>$"{x.Item1}={x.Item2}").JoinStrings("&");
                if (!string.IsNullOrWhiteSpace(pstr))
                {
                    uri = new Uri(uri.ToString() + "?" + pstr, uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative);
                }
            }

            var response = await client.GetAsync(uri, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Не удалось выполнить запрос")
                {
                    Data =
                    {
                        ["response"] = response.ToString(),
                        ["body"] = body
                    }
                };
            return body.ToObject<TResponse>(options);
        }
    }
}
