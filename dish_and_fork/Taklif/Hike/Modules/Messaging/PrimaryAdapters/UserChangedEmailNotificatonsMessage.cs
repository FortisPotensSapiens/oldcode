using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record UserChangedEmailNotificatonsMessage(string ConfirmationCode, EmailVo NewEmail, UserId UserId) : NotificatonsMessage;
}
