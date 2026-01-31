

using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Twilio.Rest.Verify.V2.Service;

namespace Hike.Clients
{
    public enum VerivicationStatus
    {
        Pending = 10,
        Approved,
        Canceled
    }

    public interface ITwilioClient
    {
        Task<VerivicationStatus> SendVerificationToken(string phone);
        Task<VerivicationStatus> CheckVerificationCode(string phone, string code);
    }

    public class TwilioClient : ITwilioClient
    {
        private readonly string _pathSeviceSid;
        private IDistributedCache _cache;
        public TwilioClient(string pathSeviceSid, IDistributedCache cache)
        {
            _pathSeviceSid = pathSeviceSid;
            _cache = cache;
        }

        public async Task<VerivicationStatus> CheckVerificationCode(string phone, string code)
        {
            var old = await _cache.GetStringAsync(phone + code);
            if (old == "true")
                return VerivicationStatus.Approved;
            var verificationCheck = await VerificationCheckResource.CreateAsync(
         to: phone,
         code: code,
         pathServiceSid: _pathSeviceSid
     );
            var status = ToStatus(verificationCheck.Status);
            await _cache.SetStringAsync(phone + code, (status == VerivicationStatus.Approved).ToString().ToLower());
            return status;
        }

        public async Task<VerivicationStatus> SendVerificationToken(string phone)
        {
            var verification = await VerificationResource.CreateAsync(
          to: phone,
          channel: "sms",
          pathServiceSid: _pathSeviceSid
      );
            return ToStatus(verification.Status);
        }


        private static VerivicationStatus ToStatus(string verificationCheck)
        {
            return verificationCheck switch
            {
                "approved" => VerivicationStatus.Approved,
                "canceled" => VerivicationStatus.Canceled,
                _ => VerivicationStatus.Pending
            };
        }
    }
}
