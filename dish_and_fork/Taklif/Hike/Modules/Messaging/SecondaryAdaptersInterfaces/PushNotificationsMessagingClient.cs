using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.MessagingModule.SecondaryAdaptersInterfaces;
using Hike.Clients;

namespace Infrastructure.SecondaryAdapters.Clients
{
    public class PushNotificationsMessagingClient : IPushNotificationsMessagingClient
    {
        private readonly IPushNotificationsClient _fcm;

        public PushNotificationsMessagingClient(IPushNotificationsClient fcm)
        {
            _fcm = fcm;
        }

        public Task SendPushNotifications(PushToken pushToken, PushNotificatonsMessage message)
        {
            return message switch
            {
                PlainTextMessage ptm => PlainTextMessage(pushToken, ptm),
                PlaintTextWithDescriptionMessage ptmwd => PlaintTextWithDescriptionMessage(pushToken, ptmwd),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task PlaintTextWithDescriptionMessage(PushToken pushToken, PlaintTextWithDescriptionMessage message)
        {
            await _fcm.SendAsync(pushToken, new FcmRequest
            {
                Message = new FcmMessage
                {
                    Notification = new FcmNotification
                    {
                        Title = message.Txt,
                        Body = message.Description
                    }
                }
            });
        }

        public async Task PlainTextMessage(PushToken pushToken, PlainTextMessage message)
        {
            await _fcm.SendAsync(pushToken, new FcmRequest
            {
                Message = new FcmMessage
                {
                    Notification = new FcmNotification
                    {
                        Title = message.Txt,
                        Body = message.Txt
                    }
                }
            });
        }
    }
}
