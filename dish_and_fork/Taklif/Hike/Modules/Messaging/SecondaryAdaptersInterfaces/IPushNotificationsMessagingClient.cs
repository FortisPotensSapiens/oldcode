using System.Threading.Tasks;
using Daf.MessagingModule.Domain;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface IPushNotificationsMessagingClient
    {
        Task SendPushNotifications(PushToken pushToken, PushNotificatonsMessage message);
    }
}
