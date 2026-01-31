using Microsoft.AspNetCore.Http;

namespace Daf.MessagingModule.Domain
{
    public record SendFeedbackEmailMessage(string Name, string Email, string Message, IEnumerable<IFormFile> Files) : EmailMessage;
}
