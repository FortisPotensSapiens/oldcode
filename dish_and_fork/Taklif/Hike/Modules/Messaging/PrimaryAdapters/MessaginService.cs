using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.MessagingModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public class MessaginService : IMessaginService
    {
        private readonly IPushNotificationsMessagingClient _push;
        private readonly IWebSocketsMessaginClient _webSocket;
        private readonly IDevicesRepository _devices;
        private readonly ILogger<MessaginService> _logger;
        private readonly IGuidsRepository _guids;
        private readonly IEmailMessagingClient _email;
        private readonly IDistributedCache _cache;
        private readonly ISmsMessageClient _sms;
        private readonly IChatMessagesRepository _messages;
        private readonly IDateTimesRepository _dateTimes;

        public MessaginService(
             IPushNotificationsMessagingClient push,
            IWebSocketsMessaginClient webSocket,
            IDevicesRepository devices,
            ILogger<MessaginService> logger,
            IGuidsRepository guids,
            IEmailMessagingClient email,
            IDistributedCache cache,
            ISmsMessageClient sms,
            IChatMessagesRepository messages,
            IDateTimesRepository dateTimes
            )
        {
            _push = push;
            _webSocket = webSocket;
            _devices = devices;
            _logger = logger;
            _guids = guids;
            _email = email;
            _cache = cache;
            _sms = sms;
            _messages = messages;
            _dateTimes = dateTimes;
        }

        public async Task<DeviceId> AddDevice(UserId userId, PushToken pushToken)
        {
            var id = _guids.GetNew();
            var device = new Device(id, pushToken, userId);
            await _devices.Create(new[] { device });
            return id;
        }


        public Task Delete(DeviceId id)
        {
            return _devices.Delete(new[] { id });
        }

        public async Task<Device?> Get(DeviceId id)
        {
            var es = await _devices
               .GetFiltered(x => x.Devices.Where(x => x.Id == id.Value));
            return es.FirstOrDefault();
        }

        public Task Update(Device device)
        {
            return _devices.Update(new[] { device });
        }

        public Task SendNotificationMessage(NotificatonsMessage message)
        {
            if (message is OrderDelivered m)
                return SendOrderDelivered(m.UserIds, m);
            if (message is OfferCommentAddedMessage commentAdded)
                return SendOfferCommentAdded(commentAdded.UserIds, commentAdded);
            if (message is SendedFeedbackMessage sendedFeedback)
                return SendSendedFeedback(sendedFeedback);
            if (message is SendFeedbackMessageFromLanding sendFromLending)
                return SendSendedFeedbackFromLanding(sendFromLending);
            if (message is SendPhoneVeficationMessage phoneVeficationMessage)
                return SendSmsVerificationCode(phoneVeficationMessage);
            if (message is OrderPaidMessage orderPaid)
                return SendOrderPaid(orderPaid.UserIds, orderPaid);
            if (message is UserChangedEmailNotificatonsMessage userChangedEmail)
                return UserChangedEmail(userChangedEmail);
            if (message is ResendEmailConfirmationRequestedNotificatonsMessage recr)
                return UserRequstedResendEmailConfirmation(recr);
            if (message is UserRegisteredNotificatonsMessage urnm)
                return UserRegistered(urnm);
            if (message is UserResetedPasswordRequestedNotificatonsMessage urpnm)
                return UserResetedPassword(urpnm);

            throw new NotImplementedException();
        }

        private async Task UserResetedPassword(UserResetedPasswordRequestedNotificatonsMessage message)
        {
            await _email.SendEmailMessage(new UserResetedPasswordRequestedEmailMessage(
                message.Code,
                message.ReturnUrl,
                message.Email
                ));
        }

        private async Task UserRegistered(UserRegisteredNotificatonsMessage message)
        {
            await _email.SendEmailMessage(new UserRegisteredEmailMessage(
                message.ConfirmationCode,
                message.UserId,
                message.NewEmail,
                message.ReturnUrl
                ));
        }

        private async Task UserRequstedResendEmailConfirmation(ResendEmailConfirmationRequestedNotificatonsMessage message)
        {
            await _email.SendEmailMessage(new ResendEmailConfirmationRequestedEmailMessage(
                message.Code,
                message.UserId,
                message.Email
                ));
        }

        private async Task UserChangedEmail(UserChangedEmailNotificatonsMessage message)
        {
            await _email.SendEmailMessage(new UserChangedEmailEmailMessage(message.ConfirmationCode, message.NewEmail, message.UserId));
        }

        private async Task SendSmsVerificationCode(SendPhoneVeficationMessage message)
        {
            await CheckTimer(message.Phone);
            await _sms.SendSmsMessage(new SendPhoneVerificationSmsMessage(message.Phone));
        }

        private async Task SendSendedFeedbackFromLanding(SendFeedbackMessageFromLanding m)
        {
            await CheckTimer(m.Email);
            await _email.SendEmailMessage(new SendFeedbackEmailMessageFromLanding(m.LendingMessageType, m.Name, m.Email, m.Message, m.Files));
        }

        private async Task SendSendedFeedback(SendedFeedbackMessage m)
        {
            await CheckTimer(m.Email);
            await _email.SendEmailMessage(new SendFeedbackEmailMessage(m.Name, m.Email, m.Message, m.Files));
        }

        private async Task SendOfferCommentAdded(IEnumerable<UserId> userIds, OfferCommentAddedMessage message)
        {
            await SendToWebSocketAndPush(
              userIds,
              new PlaintTextWithDescriptionMessage("Новый коментарий к вашему отклику", message.Text),
              new OfferCommentAddedWebSocketMessage(message.OfferId, message.Text, message.CommentId)
              );
        }

        private async Task SendOrderDelivered(
            IEnumerable<UserId> userIds,
            OrderDelivered message
            )
        {
            await SendToWebSocketAndPush(
                userIds,
                new PlainTextMessage($"Ваш заказ номер {message.OrderNumber.Value} Доставлен!"),
                new OrderDeliveredWebSocketMessage()
                );
        }

        private async Task SendOrderPaid(
         IEnumerable<UserId> userIds,
         OrderPaidMessage message
         )
        {
            await SendToWebSocketAndPush(
                userIds,
                new PlainTextMessage($"Ваш заказ номер {message.OrderNumber.Value} Оплачен!"),
                new OrderPaidWebSocketMessage()
                );
            await _email.SendEmailMessage(new OrderPaidEmailMessage(message.OrderNumber, message.StoreEmail, message.Items));
        }

        private async Task SendToWebSocketAndPush(
            IEnumerable<UserId> userIds,
            PushNotificatonsMessage pushNotificatonsMessage,
            WebSocketMessage socketMessage
            )
        {
            try
            {
                var us = userIds.ToList();
                var ug = us.Select(x => (string)x).ToList();
                var devices = await _devices.GetFiltered(db => db.Devices.Where(x => ug.Contains(x.UserId)));
                foreach (var d in devices)
                {
                    await _push.SendPushNotifications(d.Token, pushNotificatonsMessage);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on send push {@Notification}", pushNotificatonsMessage);
            }
            try
            {
                foreach (var id in userIds)
                {
                    await _webSocket.SendWebSocketMessage(id, socketMessage);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on send web socket {@Notification}", socketMessage);
            }
        }

        public async Task CheckTimer(string key)
        {
            if (DateTime.TryParse(await _cache.GetStringAsync(key), out var lastSend) && (DateTime.UtcNow - lastSend).TotalSeconds < 30)
                new { key = key }.ThrowApplicationException("Подождите 30 секунд прежде чем послать следующее сообщение!");
            await _cache.SetStringAsync(key, DateTime.UtcNow.ToString("u"));
        }

        public async Task<bool> CheckCodeIsValid(string code, PhoneNumber phone)
        {
            var result = await _cache.GetStringAsync(code + phone);
            if (result == "true")
                return true;
            var r = await _sms.CheckCodeIsValid(code, phone);
            await _cache.SetStringAsync(code + phone, r.ToString().ToLower());
            return r;
        }

        public Task<List<ChatMessage>> GetByChatId(ChatId id)
        {
            return _messages.GetFiltered(db => db.Comments.Where(x => x.OfferId == id.Value));
        }

        public async Task<ChatMessageId> Create(ChatId chatId, ChatMessageId? parentId, LongText text, UserId userId)
        {
            ChatMessageId id = _guids.GetNew();
            NotDefaultDateTime created = _dateTimes.Now();
            var message = new ChatMessage(id, chatId, text, userId, parentId, created);
            await _messages.Create(new[] { message });
            return id;
        }

        public Task<List<Device>> Get(PushToken token)
        {
            return _devices.GetFiltered(x => x.Devices.Where(d => d.FcmPushToken == token.Value));
        }

        public Task<List<Device>> Get(PageNumber pageNumber, PageSize pageSize, UserId userId)
        {
            return _devices.GetFiltered(db => db.Devices.AsNoTracking()
            .Where(x => x.UserId == userId.Value)
            .Skip((pageNumber.Value - 1) * pageSize.Value)
            .Take(pageSize.Value));
        }

        public Task<BigCount> CountDevices(UserId userId)
        {
            return _devices.GetCount(db => db.Devices.AsNoTracking()
            .Where(x => x.UserId == userId.Value));
        }
    }
}
