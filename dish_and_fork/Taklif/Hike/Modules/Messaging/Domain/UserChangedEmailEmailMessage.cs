using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{
    public record UserChangedEmailEmailMessage(string ConfirmationCode, EmailVo NewEmail, UserId UserId) : EmailMessage;
}
