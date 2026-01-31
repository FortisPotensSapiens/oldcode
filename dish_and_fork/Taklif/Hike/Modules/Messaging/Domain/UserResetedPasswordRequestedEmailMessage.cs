using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{
    public record UserResetedPasswordRequestedEmailMessage(
string Code,
string ReturnUrl,
EmailVo Email
) : EmailMessage;
}
