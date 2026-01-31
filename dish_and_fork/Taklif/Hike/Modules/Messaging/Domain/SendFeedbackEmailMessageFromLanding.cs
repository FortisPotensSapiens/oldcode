using Microsoft.AspNetCore.Http;

namespace Daf.MessagingModule.Domain
{
    public record SendFeedbackEmailMessageFromLanding(LendingMessageType LendingMessageType, string Name, string Email, string Message, IEnumerable<IFormFile> Files)
       : EmailMessage;
}
