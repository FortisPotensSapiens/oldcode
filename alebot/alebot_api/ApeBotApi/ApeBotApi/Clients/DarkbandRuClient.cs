using System.Threading.Tasks;
using ApeBotApi.Extensions;

namespace AleBotApi.Clients
{
    public interface IDarkbandRuClient
    {
        public Task<(Dictionary<string, DbrLicensePrice> licensePrices, Dictionary<string, DbrVpsServerPice> vpsServerPirces)> GetTarifs();
        public Task<(List<DbrProduct> bots, DbrVps vps)> BuyLicenseWithServer(DbrLicenseTarifs tariff, Guid userId);
        public Task UpdateAccountNumber(int botId, string accountNumber);
        public Task<DbrVps> GetVpsInformation(int vpsId);
        public Task RebootVps(int vpsId);
        public Task<(DbrVps vps, decimal amount)> ExtendVps(int vpsId, DbrVpsServersTariffs tariff);
        public Task<(List<DbrBotAccount> accounts, List<DbrVps> vps)> GetUserInformation(Guid userId);
        public Task<DbrBotAccount> GetBotAccountInformation(int accountId);
        public Task<DbrVps> BuyOnlyVpsServer(Guid userId, DbrVpsServersTariffs tariff);
    }

    public class DarkbandRuClient : IDarkbandRuClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _hc;

        public DarkbandRuClient(HttpClient hc, IConfiguration cfg)
        {
            _apiKey = cfg["Darkband:ApiKey"];
            _hc = hc;
        }

        public async Task<(List<DbrProduct> bots, DbrVps vps)> BuyLicenseWithServer(DbrLicenseTarifs tariff, Guid userId)
        {
            var uri = $"/buy?tariff={(int)tariff}&user_id={userId}&api_key={_apiKey}";
            var response = await Get<DbrBuyLicenseWithServerResponse>(uri);
            return (response.bots, response.vps);
        }

        public async Task<DbrVps> BuyOnlyVpsServer(Guid userId, DbrVpsServersTariffs tariff)
        {
            var uri = $"/vps-buy?tariff={(int)tariff}&user_id={userId}&api_key={_apiKey}";
            var response = await Get<DbrBuyOnlyVpsServerResponse>(uri);
            return response.vps;
        }

        public async Task<(DbrVps vps, decimal amount)> ExtendVps(int vpsId, DbrVpsServersTariffs tariff)
        {
            var uri = $"/vps-extention?tariff={(int)tariff}&vps_id={vpsId}&api_key={_apiKey}";
            var response = await Get<DbrBuyOnlyVpsServerResponse>(uri);
            return (response.vps, response.amount);
        }

        public async Task<DbrBotAccount> GetBotAccountInformation(int accountId)
        {
            var uri = $"/account-information?account_id={accountId}&api_key={_apiKey}";
            var response = await Get<DbrGetAccountInformationResponse>(uri);
            return response.account;
        }

        public async Task<(Dictionary<string, DbrLicensePrice> licensePrices, Dictionary<string, DbrVpsServerPice> vpsServerPirces)> GetTarifs()
        {
            var uri = $"/get-tariffs?api_key={_apiKey}";
            var response = await Get<DbrGetTarifsResponse>(uri);
            return (response.tariffs, response.vps);
        }

        public async Task<(List<DbrBotAccount> accounts, List<DbrVps> vps)> GetUserInformation(Guid userId)
        {
            var uri = $"/user-information?api_key={_apiKey}&user_id={userId}";
            var response = await Get<DbrGetUserInformationResponse>(uri);
            return (response.accounts, response.vps);
        }

        public async Task<DbrVps> GetVpsInformation(int vpsId)
        {
            var uri = $"/vps-information?api_key={_apiKey}&vps_id={vpsId}";
            var response = await Get<DbrGetVpsInformationResponse>(uri);
            return response.vps;
        }

        public async Task RebootVps(int vpsId)
        {
            var uri = $"/vps-reboot?api_key={_apiKey}&vps_id={vpsId}";
            var response = await Get<DbrResponseBase>(uri);
        }

        public async Task UpdateAccountNumber(int botId, string accountNumber)
        {
            var uri = $"/update-account-number?api_key={_apiKey}&account_number={accountNumber}&account_id={botId}";
            var response = await Get<DbrResponseBase>(uri);
        }

        private async Task<T> Get<T>(string uri) where T : DbrResponseBase
        {
            var response = await _hc.GetJsonAsync<T>(new Uri(uri, UriKind.Relative));
            if (!response.success)
                new { uri, response }.ThrowApplicationException("Ошибка при запросе к darkband.ru!");
            return response;
        }
    }

    public class DbrGetUserInformationResponse : DbrResponseBase
    {
        public List<DbrBotAccount> accounts { get; set; } = new();
        public List<DbrVps> vps { get; set; } = new();
    }



    public class DbrGetTarifsResponse : DbrResponseBase
    {

        public Dictionary<string, DbrLicensePrice> tariffs { get; set; } = new();
        public Dictionary<string, DbrVpsServerPice> vps { get; set; } = new();
    }

    public class DbrGetVpsInformationResponse : DbrResponseBase
    {
        public DbrVps? vps { get; set; }
    }
    public class DbrGetAccountInformationResponse : DbrResponseBase
    {
        public DbrBotAccount? account { get; set; }
    }

    public class DbrBuyLicenseWithServerResponse : DbrResponseBase
    {
        public List<DbrProduct> bots { get; set; } = new();
        public DbrVps vps { get; set; }
        public decimal amount { get; set; }
    }

    public class DbrBuyOnlyVpsServerResponse : DbrResponseBase
    {
        public DbrVps vps { get; set; }
        public decimal amount { get; set; }
    }

    public class DbrBotAccount
    {
        public int? account_id { get; set; }
        public int? product_id { get; set; }
        public string product { get; set; }
        public string last_ping { get; set; }
        public string expiration { get; set; }
        public int? number { get; set; }
        public int? real { get; set; }
        public int? leverage { get; set; }
        public string server { get; set; }
        public int? balance { get; set; }
        public int? equity { get; set; }
        public int? margin { get; set; }
        public int? max_drawdown { get; set; }
        public int? activated { get; set; }
    }

    public class DbrVps
    {
        public int? vps_id { get; set; }
        public string? expiration { get; set; }
        public string? server { get; set; }
        public string? user { get; set; }
        public string? password { get; set; }
        public string? status { get; set; }
    }


    public class DbrProduct
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public string product { get; set; }
    }

    public enum DbrLicenseTarifs
    {
        Trial = 1,
        Invest = 2,
        Worker = 3,
        VIP = 4,
    }

    public enum DbrVpsServersTariffs
    {
        OneMonth = 1,
        ThreeMonth = 2,
        SixMonth = 3,
        TwelveMonth = 4,
    }

    public class DbrResponseBase
    {
        public bool success { get; set; }
        public string? error { get; set; }
    }

    public class DbrLicensePrice
    {
        public string name { get; set; }
        public string price { get; set; }
    }

    public class DbrVpsServerPice
    {
        public int period { get; set; }
        public string price { get; set; }
    }
}
