using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{
    public record ResendEmailConfirmationRequestedEmailMessage(
      string Code,
      UserId UserId,
      EmailVo Email
      ) : EmailMessage;
}
