using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.Domain
{
    public record PlainTextMessage(NotNullOrWhitespaceString Txt) : PushNotificatonsMessage { }
}
