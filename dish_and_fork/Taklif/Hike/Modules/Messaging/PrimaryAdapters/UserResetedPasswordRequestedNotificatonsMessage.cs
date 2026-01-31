using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record UserResetedPasswordRequestedNotificatonsMessage(
string Code,
string ReturnUrl,
EmailVo Email
) : NotificatonsMessage;
}

