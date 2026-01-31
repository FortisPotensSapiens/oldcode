using Microsoft.AspNetCore.Http;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record SendedFeedbackMessage(string Name, string Email, string Message, IEnumerable<IFormFile> Files) : NotificatonsMessage;
}
