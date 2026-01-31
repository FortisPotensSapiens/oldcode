using ApeBotApi.Extensions;

namespace AleBotApi.Services
{

    public interface ICryptoCloudService
    {
        Task<CreateInvoiceResponse> CreateInvoiceAsync(Guid alebotInvoiceId, decimal amount);

        Task<bool> IsInvocePaind(string invoceId);
    }

    public class CryptoCloudService : ICryptoCloudService
    {
        private readonly IConfiguration _configuration;

        public CryptoCloudService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<CreateInvoiceResponse> CreateInvoiceAsync(Guid alebotInvoiceId, decimal amount)
        {
            string apiHost = _configuration.GetValue<string>("CryptoCloud:Host") ?? string.Empty;
            string apiKey = _configuration.GetValue<string>("CryptoCloud:ApiKey") ?? string.Empty;
            string shopId = _configuration.GetValue<string>("CryptoCloud:ShopId") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(apiHost) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(shopId))
                throw new Exception("Нет данных для подключения к платежной системе");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
            var response = await httpClient.PostAsync<CreateInvoiceResponse, CreateInvoiceRequest>(new Uri($"{apiHost}/v1/invoice/create"),
                new CreateInvoiceRequest
                {
                    shop_id = shopId,
                    order_id = alebotInvoiceId.ToString(),
                    amount = amount,
                });

            return response;
        }

        public async Task<bool> IsInvocePaind(string invoceId)
        {
            if (string.IsNullOrWhiteSpace(invoceId))
                new { invoceId }.ThrowApplicationException("Укажите номер счета на оплату в крипто клауде!");
            string apiHost = _configuration.GetValue<string>("CryptoCloud:Host") ?? string.Empty;
            string apiKey = _configuration.GetValue<string>("CryptoCloud:ApiKey") ?? string.Empty;
            string shopId = _configuration.GetValue<string>("CryptoCloud:ShopId") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(apiHost) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(shopId))
                new { invoceId }.ThrowApplicationException("Нет данных для подключения к платежной системе");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
            var response = await httpClient.GetJsonAsync<GetInvoceStatusResponse>(new Uri($"{apiHost}/v1/invoice/info?uuid={invoceId?.Trim()}"));
            return response?.status?.GetNormalizedName() == "success".GetNormalizedName() &&
                (response?.status_invoice?.GetNormalizedName() == "paid".GetNormalizedName() || response?.status_invoice?.GetNormalizedName() == "overpaid".GetNormalizedName());
        }
    }

    public class CreateInvoiceRequest
    {
        public string shop_id { get; set; } = null!;
        public string order_id { get; set; } = null!;
        public decimal amount { get; set; }
    }

    public class CreateInvoiceResponse
    {
        public string status { get; set; } = null!;
        public string pay_url { get; set; } = null!;
        public string currency { get; set; } = null!;
        public string invoice_id { get; set; } = null!;
        public decimal amount { get; set; }
        public decimal amount_usd { get; set; }
    }

    public class GetInvoceStatusResponse
    {
        public string status { get; set; }
        public string status_invoice { get; set;}
    }
}
