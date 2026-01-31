using Hike.Entities;

namespace Hike.Models
{
    public class ConfigModel
    {
        public string SsoUri { get; set; } = string.Empty;
        public string ApiUri { get; set; } = string.Empty;
        public bool IsTesting { get; set; }
        public CurrencyType Currency { get; set; } = CurrencyType.Rub;
        public string TermOfServiceFilePath { get; set; }
        public string PartnerTermOfServiceFilePath { get; set; }
        public double CommissionPrecentageForMerch { get; set; }
        /// <summary>
        /// Разрешение отправлять пользователю рассылки
        /// </summary>
        public string ConsentToMailingsFilePath { get; set; }
        /// <summary>
        /// Политика конфеденциальности
        /// </summary>
        public string PrivacyPolicyFilePath { get; set; }
        /// <summary>
        /// Согласие на обработку персональных данных
        /// </summary>
        public string ConsentToPersonalDataProcessingFilePath { get; set; }
        public string OfferForBuyerFilePath { get; set; }
        public string TermsOfUserFilePath { get; set; }
    }
}
