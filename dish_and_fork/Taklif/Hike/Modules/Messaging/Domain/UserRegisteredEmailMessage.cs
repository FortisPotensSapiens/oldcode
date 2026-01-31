using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{
    public record UserRegisteredEmailMessage(
  string Code,
  UserId UserId,
  EmailVo Email,
  string ReturnUrl
  ) : EmailMessage;
}
