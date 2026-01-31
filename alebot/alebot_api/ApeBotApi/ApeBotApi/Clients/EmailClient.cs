using MailKit.Net.Smtp;
using MimeKit;

namespace AleBotApi.Clients
{
    public class EmailClient : IEmailClient
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("AleBot", "alebot.info@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(email, email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate("alebot.info@gmail.com", "nhfo wtur guxu lxau");
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);
            }
        }
    }
}
