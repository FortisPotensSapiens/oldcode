using System.Threading.Tasks;
using Hike.Extensions;
using Hike.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hike.Services
{
    public interface IEmailClient : IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message,
            List<(string name, string type, byte[] content)> attachment);
    }
    public class EmailSender : IEmailClient
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly AuthMessageSenderOptions _options;
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await SendEmailAsync(email, subject, message, new List<(string name, string type, byte[] content)>());
        }

        public async Task SendEmailAsync(string email, string subject, string message, List<(string name, string type, byte[] content)> attachment)
        {
            _logger.LogInformation("Отправляем почту {Email} {Subject} {Message}, {SendGridKey}", email, subject, message, _options?.SendGridKey);
            var client = new SendGridClient(_options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.SendGridEmail, _options.SendGridName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            if (attachment != null && attachment.Count != 0)
                msg.Attachments = attachment.Select(a => new Attachment()
                {
                    Content = Convert.ToBase64String(a.content),
                    Filename = a.name,
                    Type = a.type
                }).ToList();
            msg.AddTo(new EmailAddress(email));
            // https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException(await response.Body.ReadAsStringAsync());
            _logger.LogInformation("Оправили почту {Msg}", msg.ToJson());
        }
    }
}
