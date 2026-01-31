using Daf.MessagingModule.Domain;
using Microsoft.AspNetCore.Http;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record SendFeedbackMessageFromLanding(LendingMessageType LendingMessageType, string Name, string Email, string Message, IEnumerable<IFormFile> Files)
      : NotificatonsMessage;
}
