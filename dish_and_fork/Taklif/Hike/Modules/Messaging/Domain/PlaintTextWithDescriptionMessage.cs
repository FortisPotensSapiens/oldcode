using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.Domain
{
    public record PlaintTextWithDescriptionMessage(NotNullOrWhitespaceString Txt, NotNullOrWhitespaceString Description) : PushNotificatonsMessage { }
}
