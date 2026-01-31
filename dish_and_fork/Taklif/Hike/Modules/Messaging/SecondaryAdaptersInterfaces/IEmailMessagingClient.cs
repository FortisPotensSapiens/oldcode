using System.Threading.Tasks;
using Daf.MessagingModule.Domain;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface IEmailMessagingClient
    {
        Task SendEmailMessage(EmailMessage message);
    }
}
