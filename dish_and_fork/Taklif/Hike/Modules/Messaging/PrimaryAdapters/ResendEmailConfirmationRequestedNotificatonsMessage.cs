using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record ResendEmailConfirmationRequestedNotificatonsMessage(
        string Code,
        UserId UserId,
        EmailVo Email) : NotificatonsMessage
    { }
}
