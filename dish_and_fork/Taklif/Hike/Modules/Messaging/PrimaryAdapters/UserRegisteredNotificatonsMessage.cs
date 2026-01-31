using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record UserRegisteredNotificatonsMessage(string ConfirmationCode, EmailVo NewEmail, UserId UserId, string ReturnUrl) : NotificatonsMessage;
}
