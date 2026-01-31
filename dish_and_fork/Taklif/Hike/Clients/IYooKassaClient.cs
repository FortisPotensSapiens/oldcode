using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Hike.Extensions;
using Microsoft.Extensions.Configuration;

namespace Hike.Clients
{
    public interface IYooKassaClient
    {
        Task<(string id, string redirect)> CratePayment(string key, decimal dp, decimal ip, string returnUrl, string description,
                    string iId);
        Task<string> GetPaymentStatus(string id);
    }

    public class YooKassaClient : IYooKassaClient
    {
        private const string ApplicationJson = "application/json";
        private readonly string _authorization;
        private readonly string _apiUrl;
        private readonly HttpClient _client;
        private readonly string _deliveryId;
        private readonly string _shopId;
        public YooKassaClient(HttpClient client, IConfiguration configuration)
        {
            var id = configuration["YooKassa:MyShopId"];
            var password = configuration["YooKassa:MySecretKey"];
            _authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(
                id + ":" + password
                ));
            _apiUrl = "https://api.yookassa.ru/v3/";
            _client = client;
            _deliveryId = configuration["YooKassa:DeliveryShopId"];
            _shopId = configuration["YooKassa:MyShopId"];
        }

        public async Task<string> GetPaymentStatus(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                new { }.ThrowApplicationException("Укажите id платежа в YooKassa!");
            var request = CreateRequest(HttpMethod.Get, null, $"payments/{id}", null);
            var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                new { body }.ThrowApplicationException("Проблемы с Доставистой!");
            var jo = JsonObject.Parse(body);
            return jo["status"].AsValue().ToString();
        }
        public async Task<(string id, string redirect)> CratePayment(string key, decimal dp, decimal ip, string returnUrl, string description,
            string iId)
        {
            dp = dp < 1 ? 0 : dp;
            var request = CreatePaymentRequest(key, new
            {
                amount = new
                {
                    value = (ip + dp).ToString("##.##"),
                    currency = "RUB"
                },
                capture = true,
                confirmation = new
                {
                    type = "redirect",
                    return_url = returnUrl
                },
                description = description,
                transfers = dp < 1 ? (new object[] {
                   new {
                        account_id = iId, //Тут для селф деливеред должен быть Id магазина кондитера а не доставки
                         amount = new
                         {
                             value = (ip + dp).ToString("##.##"),
                             currency = "RUB"
                         }
                    } }) : (new object[] {
                    new
                    {
                        account_id = iId, // Тут должен быть Id магазина кондитера
                         amount = new
                         {
                             value = (ip).ToString("##.##"),
                             currency = "RUB"
                         }
                    },
                   new {
                        account_id = _deliveryId, // это Id магазина доставки
                         amount = new
                         {
                             value = dp.ToString("##.##"),
                             currency = "RUB"
                         }
                    }
                })
            });

            var response = await _client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                new { body }.ThrowApplicationException("Проблемы с YooKassa!");
            var jo = JsonObject.Parse(body);
            return (jo["id"].AsValue().ToString(), jo["confirmation"]["confirmation_url"].AsValue().ToString());
        }

        private HttpRequestMessage CreatePaymentRequest(string key, object data)
        => CreateRequest(HttpMethod.Post, data, "payments ", key);

        private HttpRequestMessage CreateRequest(HttpMethod method, object body, string url, string idempotenceKey)
        {
            var request = new HttpRequestMessage(method, _apiUrl + url);
            request.Headers.Add("Authorization", _authorization);
            if (!string.IsNullOrEmpty(idempotenceKey))
                request.Headers.Add("Idempotence-Key", idempotenceKey);
            if (body != null)
            {
                string json = body.ToJson();
                request.Content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            }
            return request;
        }

    }
}
