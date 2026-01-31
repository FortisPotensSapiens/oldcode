using System.IO;
using Newtonsoft.Json;

namespace Hike.Modules.AdminSettings
{
    public class AdminSettingsDto
    {
        public double? MinDeliveryPrice { get; set; }
        public double? DeliveryCoefficient { get; set; }
        public double? MinMerchPrice { get; set; }
        public double? MerchCoefficient { get; set; }

        public decimal CalculateDeliveryTotalPrice(decimal basePrice)
        {
            var c = (decimal)(DeliveryCoefficient ?? 0) / 100M;
            var p = (decimal)(MinDeliveryPrice ?? 0);
            return CaculateOurCommission(basePrice, c, p) + basePrice;
        }

        public decimal CalculateOrderTotalPrice(decimal basePrice)
        {
            var c = (decimal)(MerchCoefficient ?? 0) / 100M;
            var p = (decimal)(MinMerchPrice ?? 0);
            return CaculateOurCommission(basePrice, c, p) + basePrice;
        }

        private decimal CaculateOurCommission(decimal basePrice, decimal coefficient, decimal minCommission)
        {
            var k = coefficient * basePrice;
            if (minCommission > k)
                k = minCommission + k;
            return k;
        }
    }
}
