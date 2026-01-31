using System.Security.Cryptography;
using System.Text;
using Hike.Extensions;

namespace Hike.Controllers
{
    public partial class PaymentsController
    {
        public class PayAnyWayCallbackModel
        {
            public string MNT_ID { get; set; }
            public string MNT_TRANSACTION_ID { get; set; }
            public string MNT_OPERATION_ID { get; set; }
            public string MNT_AMOUNT { get; set; }
            public string MNT_CURRENCY_CODE { get; set; }
            public string MNT_SUBSCRIBER_ID { get; set; }
            public string MNT_TEST_MODE { get; set; }
            public string MNT_SIGNATURE { get; set; }

            public bool IsValid() => MNT_SIGNATURE.GetNormalizedName() == GetSignature().GetNormalizedName();

            public string GetSignature()
            {
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(MNT_ID + MNT_TRANSACTION_ID + MNT_OPERATION_ID + MNT_AMOUNT + MNT_CURRENCY_CODE + MNT_SUBSCRIBER_ID + MNT_TEST_MODE + 12345));
                    return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
                }
            }
        }
    }
}
