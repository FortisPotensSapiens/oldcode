using Hike.Modules.AdminSettings;

namespace Hike.Models
{
    public sealed class GlobaSettingsModel
    {
        /// <summary>
        /// Минимальная сумма доставки
        /// </summary>
        [Range(0, 1000000)]
        public double MinDeliveryPrice { get; set; }
        /// <summary>
        /// Коэффициент комисси для доставки в процентах
        /// </summary>
        [Range(0, 100)]
        public double DeliveryCoefficient { get; set; }
        /// <summary>
        /// Минимальная сумма товаров
        /// </summary>
        [Range(0, 1000000)]
        public double MinMerchPrice { get; set; }
        /// <summary>
        /// Коэффициент коммисси для суммы товаров в процентах
        /// </summary>
        [Range(0, 100)]
        public double MerchCoefficient { get; set; }

        public GlobaSettingsModel()
        {

        }

        public GlobaSettingsModel(AdminSettingsDto dto)
        {
            MinDeliveryPrice = dto.MinDeliveryPrice ?? 0;
            DeliveryCoefficient = dto.DeliveryCoefficient ?? 0;
            MinMerchPrice = dto.MinMerchPrice ?? 0;
            MerchCoefficient = dto.MerchCoefficient ?? 0;
        }

        public AdminSettingsDto ToSettings()
        {
            return new AdminSettingsDto
            {
                MerchCoefficient = MerchCoefficient,
                MinMerchPrice = MinMerchPrice,
                DeliveryCoefficient = DeliveryCoefficient,
                MinDeliveryPrice = MinDeliveryPrice
            };
        }
    }
}
