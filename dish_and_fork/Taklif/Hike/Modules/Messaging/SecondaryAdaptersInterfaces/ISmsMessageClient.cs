using System.Threading.Tasks;
using Daf.MessagingModule.Domain;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface ISmsMessageClient
    {
        Task SendSmsMessage(SmsMessage message);
        Task<bool> CheckCodeIsValid(string code, string phone);
    }
}
