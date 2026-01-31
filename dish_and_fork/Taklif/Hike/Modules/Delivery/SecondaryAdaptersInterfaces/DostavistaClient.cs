using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Daf.DeliveryModule.Domain;
using Daf.DeliveryModule.SecondaryAdaptersInterfaces;
using Hike.Extensions;
using JoyMoe.Common.Json;

namespace Infrastructure.SecondaryAdapters.Clients.Dostavista
{
    public class DostavistaClient : IDostavistaClient
    {
        private readonly HttpClient _client;
        private const string BASE_URL = "/api/business/1.3/";
        public DostavistaClient(HttpClient client)
        {
            _client = client;
        }

        public Task<DostavistaCalculateOrderResponse> CalculateOrder(DostavistaCalculateOrderRequest request) =>
            MakeRequest<DostavistaCalculateOrderResponse, object>(new Uri($"{BASE_URL}calculate-order", UriKind.Relative),
                new
                {
                    type = request.Type.ToString("G"),
                    request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    request.Points,
                });

        public Task<DostavistaCalculateOrderResponse> CreateOrder(DostavistaCalculateOrderRequest request) =>
            MakeRequest<DostavistaCalculateOrderResponse, object>(new Uri($"{BASE_URL}create-order", UriKind.Relative),
                new
                {
                    type = request.Type.ToString("G"),
                    request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    request.Points
                });

        public Task<DostavistaCancelOrderResponse> CancelOrder(uint orderId) =>
                MakeRequest<DostavistaCancelOrderResponse, object>(new Uri($"{BASE_URL}cancel-order", UriKind.Relative),
                    new { OrderId = (int)orderId });

        public async Task<DostavistaOrder> GetOrder(uint orderId)
        {
            var response = await MakeRequest<DostavistaFindOrdersResponse, object>(new Uri($"{BASE_URL}orders", UriKind.Relative),
                  new { OrderId = (int)orderId });
            return response?.Orders?.FirstOrDefault();
        }

        public Task<DostavistaFindOrdersResponse> GetOrder(DostavistaFindOrdersRequest request) =>
            MakeRequest<DostavistaFindOrdersResponse, DostavistaFindOrdersRequest>(new Uri($"{BASE_URL}orders", UriKind.Relative), request);

        private async Task<TResponse> MakeRequest<TResponse, TRequest>(Uri uri, TRequest request) where TResponse : DostavistaResponseBase
        {
            var joptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            };
            joptions.Converters.Add(new JsonStringEnumConverter());
            var response = await _client.PostAsync<TResponse, TRequest>(uri, request, joptions);
            if (response == null || !response.IsSuccessful)
                throw new ApplicationException("Не успешный запрос к Достависте")
                {
                    Data = { ["response"] = response }
                };
            return response;
        }

        public Task<DostavistaGetIntervalsResponse> GetDeliveryIntervals(DateTime? date = null)
        {
            var joptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            };
            joptions.Converters.Add(new JsonStringEnumConverter());
            if (!date.HasValue)
                return _client.GetJsonAsync<DostavistaGetIntervalsResponse>(new Uri($"{BASE_URL}delivery-intervals", UriKind.Relative), options: joptions);
            return _client.GetJsonAsync<DostavistaGetIntervalsResponse>(new Uri($"{BASE_URL}delivery-intervals", UriKind.Relative), new[] { ("date", (object)$"{(date.Value < DateTime.UtcNow ? DateTime.UtcNow : date.Value):yyyy-MM-dd}") }, joptions);
        }
    }
}
