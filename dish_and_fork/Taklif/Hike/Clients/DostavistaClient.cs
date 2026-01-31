using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hike.Extensions;
using JoyMoe.Common.Json;

namespace Hike.Clients
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
          request.TotalWeightKg < 15
            && request.Points.Where(x => x.RequiredStartDatetime.HasValue).First().RequiredStartDatetime.Value.Day == DateTime.Now.Day
            && request.Points.Where(x => x.RequiredStartDatetime.HasValue).First().RequiredStartDatetime.Value.Hour > 9
            ? MakeRequest<DostavistaCalculateOrderResponse, Object>(new Uri($"{BASE_URL}calculate-order", UriKind.Relative),
                new
                {
                    type = DostavistaOrderType.same_day.ToString("G"),
                    matter = request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    points = request.Points
                }) : MakeRequest<DostavistaCalculateOrderResponse, Object>(new Uri($"{BASE_URL}calculate-order", UriKind.Relative),
                new
                {
                    type = DostavistaOrderType.standard.ToString("G"),
                    matter = request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    points = request.Points.Select(p =>
                    {
                        p.RequiredStartDatetime = null;
                        p.RequiredFinishDatetime = null;
                        return p;
                    }),
                    vehicle_type_id = GetVechle(request.TotalWeightKg)
                });

        public Task<DostavistaCalculateOrderResponse> CreateOrder(DostavistaCalculateOrderRequest request) =>
             request.TotalWeightKg < 15
            && request.Points.Where(x => x.RequiredStartDatetime.HasValue).First().RequiredStartDatetime.Value.Day == DateTime.Now.Day
            && request.Points.Where(x => x.RequiredStartDatetime.HasValue).First().RequiredStartDatetime.Value.Hour > 9
            ? MakeRequest<DostavistaCalculateOrderResponse, Object>(new Uri($"{BASE_URL}create-order", UriKind.Relative),
                new
                {
                    type = DostavistaOrderType.same_day.ToString("G"),
                    matter = request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    points = request.Points
                }) : MakeRequest<DostavistaCalculateOrderResponse, Object>(new Uri($"{BASE_URL}create-order", UriKind.Relative),
                new
                {
                    type = DostavistaOrderType.standard.ToString("G"),
                    matter = request.Matter,
                    total_weight_kg = request.TotalWeightKg,
                    points = request.Points.Select(p =>
                    {
                        p.RequiredStartDatetime = null;
                        p.RequiredFinishDatetime = null;
                        return p;
                    }),
                    vehicle_type_id = GetVechle(request.TotalWeightKg)
                });

        private int? GetVechle(uint weight) => weight switch
        {
            _ when weight < 15 => (int)DostavistaTransportType.Kurer,
            _ when weight < 200 => (int)DostavistaTransportType.Legcovoi,
            _ when weight < 500 => (int)DostavistaTransportType.Car500kg,
            _ when weight < 700 => (int)DostavistaTransportType.Cabluk700kg,
            _ when weight < 1000 => (int)DostavistaTransportType.MicroBus1000kg,
            _ when weight < 1500 => (int)DostavistaTransportType.Gasel1500kg,
            _ => (int)DostavistaTransportType.Gasel1500kg,
        };
        public Task<DostavistaCancelOrderResponse> CancelOrder(uint orderId) =>
                MakeRequest<DostavistaCancelOrderResponse, Object>(new Uri($"{BASE_URL}cancel-order", UriKind.Relative),
                    new { OrderId = (int)orderId });

        public async Task<DostavistaOrder> GetOrder(uint orderId)
        {
            var response = await GetRequest<DostavistaFindOrdersResponse>(new Uri($"{BASE_URL}orders?order_id={orderId}", UriKind.Relative));
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

        private async Task<TResponse> GetRequest<TResponse>(Uri uri) where TResponse : DostavistaResponseBase
        {
            var joptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            };
            joptions.Converters.Add(new JsonStringEnumConverter());
            var response = await _client.GetFromJsonAsync<TResponse>(uri, joptions);
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
            return _client.GetJsonAsync<DostavistaGetIntervalsResponse>(new Uri($"{BASE_URL}delivery-intervals", UriKind.Relative), new[] { ("date", (object)$"{date:yyyy-MM-dd}") }, joptions);
        }
    }
}
