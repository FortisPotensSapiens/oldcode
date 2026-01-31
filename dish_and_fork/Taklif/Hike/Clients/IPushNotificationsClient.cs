using System.Threading.Tasks;
using CorePush.Google;
using CorePush.Interfaces;

namespace Hike.Clients
{
    public interface IPushNotificationsClient
    {
        Task<FcmResponse> SendAsync(string deviceId, FcmRequest payload);

        Task<FcmResponse> SendAsync(FcmRequest payload);
    }

    public class PushNotificationsClient : IPushNotificationsClient
    {
        private readonly IFcmSender _fcm;
        private readonly ICancellationTokensRepository _cancellationTokens;

        public PushNotificationsClient(IFcmSender fcm, ICancellationTokensRepository cancellationTokens)
        {
            _fcm = fcm;
            _cancellationTokens = cancellationTokens;
        }

        public Task<FcmResponse> SendAsync(string deviceId, FcmRequest payload)
        {
            return _fcm.SendAsync(deviceId, payload, _cancellationTokens.GetDefault());
        }

        public Task<FcmResponse> SendAsync(FcmRequest payload)
        {
            return _fcm.SendAsync(payload, _cancellationTokens.GetDefault());
        }
    }

    public class FcmRequest
    {
        public FcmMessage Message { get; set; }
    }

    public class FcmMessage
    {
        public string? Token { get; set; }
        public string? Topic { get; set; }
        public Dictionary<string, string>? Data { get; set; }
        public FcmNotification? Notification { get; set; }
    }

    public class FcmNotification
    {
        public string Title { get; set; }
        public string? Body { get; set; }
        public string? Image { get; set; }
    }
}
