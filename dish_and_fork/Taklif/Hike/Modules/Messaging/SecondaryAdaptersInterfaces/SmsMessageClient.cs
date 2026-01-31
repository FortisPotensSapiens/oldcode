using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.MessagingModule.SecondaryAdaptersInterfaces;
using Hike.Clients;

namespace Infrastructure.SecondaryAdapters.Clients
{
    public class SmsMessageClient : ISmsMessageClient
    {
        private readonly ITwilioClient _twilio;

        public SmsMessageClient(ITwilioClient twilio)
        {
            _twilio = twilio;
        }

        public async Task<bool> CheckCodeIsValid(string code, string phone)
        {
            var res = await _twilio.CheckVerificationCode(phone, code);
            return res == VerivicationStatus.Approved;
        }

        public Task SendSmsMessage(SmsMessage message)
        {
            return message switch
            {
                SendPhoneVerificationSmsMessage spv => SendPhoneVerificationSmsMessage(spv),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task SendPhoneVerificationSmsMessage(SendPhoneVerificationSmsMessage message)
        {
            await _twilio.SendVerificationToken(message.Phone);
        }
    }
}
