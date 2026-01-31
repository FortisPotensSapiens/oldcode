using System.Threading.Tasks;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Attributes;
using Hike.Models;
using Hike.Modules.AdminSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hike.Controllers
{
    [Route("api/v1/config")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IOptionsSnapshot<ConfigModel> _configuration;
        private readonly IConfiguration _baseConfig;
        private readonly IAdminSettingsRepository _adminSettings;
        private readonly IBaseUriRepository _baseUri;

        public ConfigController(
            IOptionsSnapshot<ConfigModel> configuration,
            IConfiguration baseConfig,
            IAdminSettingsRepository adminSettings,
            IBaseUriRepository baseUri
            )
        {
            _configuration = configuration;
            _baseConfig = baseConfig;
            _adminSettings = adminSettings;
            _baseUri = baseUri;
        }

        /// <summary>
        /// Получить глобальные настрокий админа
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpGet("admin/global-settings")]
        public async Task<GlobaSettingsModel> GetGlobalSettings()
        {
            var dto = await _adminSettings.Get();
            return new GlobaSettingsModel(dto);
        }

        /// <summary>
        /// установить глобальные настройки админа
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpPut("admin/global-settings")]
        public async Task GetGlobalSettings(GlobaSettingsModel model)
        {
            await _adminSettings.Set(model.ToSettings());
        }

        /// <summary>
        /// Возврашает с сервера необходимые для Angular SPA настройки.
        /// Проверить состояние и версию API можно по адресу /health-check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ConfigModel))]
        public async Task<ConfigModel> Get()
        {
            var value = _configuration.Value;
            value.IsTesting = _baseConfig["IsTesting"]?.ToLower() == "true";
            value.TermOfServiceFilePath = new Uri(_baseUri.Get(), "/doc/terms-of-service.pdf").AbsoluteUri;
            value.PartnerTermOfServiceFilePath = new Uri(_baseUri.Get(), "/doc/terms-of-service-for-partner.pdf").AbsoluteUri;
            value.ConsentToMailingsFilePath = new Uri(_baseUri.Get(), "/doc/consent-to-mailings.pdf").AbsoluteUri;
            value.PrivacyPolicyFilePath = new Uri(_baseUri.Get(), "/doc/privacy-policy.pdf").AbsoluteUri;
            value.ConsentToPersonalDataProcessingFilePath = new Uri(_baseUri.Get(), "/doc/consent-to-personal-data-processing.pdf").AbsoluteUri;
            value.OfferForBuyerFilePath = new Uri(_baseUri.Get(), "/doc/offer-for-buyer.pdf").AbsoluteUri;
            value.TermsOfUserFilePath = new Uri(_baseUri.Get(), "/doc/terms-of-use.pdf").AbsoluteUri;
            var dto = await _adminSettings.Get();
            value.CommissionPrecentageForMerch = dto.MerchCoefficient ?? 0;
            return value;
        }

        [HttpGet]
        [Route("enums")]
        [ProducesResponseType(200, Type = typeof(Dictionary<string, List<EnumInfoModel>>))]
        public async Task<Dictionary<string, List<EnumInfoModel>>> GetEnums()
        {
            var query = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.IsEnum && t.IsPublic));
            var result = new Dictionary<string, List<EnumInfoModel>>();
            foreach (var type in query)
            {
                if (!result.ContainsKey(type.FullName) && type.FullName.ToLower().Contains("hike"))
                    result.Add(type.FullName, EnumInfoModel.Parse(type));
            }
            return result;
        }
    }
}
