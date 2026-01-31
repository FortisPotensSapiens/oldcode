using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.MessagingModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Services;

namespace Infrastructure.SecondaryAdapters.Clients
{
    public class EmailMessagingClient : IEmailMessagingClient
    {
        private readonly IEmailClient _email;
        private readonly IBaseUriRepository _baseUris;

        public EmailMessagingClient(
            IEmailClient email,
            IBaseUriRepository baseUris
            )
        {
            _email = email;
            _baseUris = baseUris;
        }

        public Task SendEmailMessage(EmailMessage message)
        {
            return message switch
            {
                UserResetedPasswordRequestedEmailMessage urprem => UserResetedPasswordRequestedEmailMessage(urprem),
                UserRegisteredEmailMessage urem => UserRegisteredEmailMessage(urem),
                ResendEmailConfirmationRequestedEmailMessage recrem => ResendEmailConfirmationRequestedEmailMessage(recrem),
                OrderPaidEmailMessage opem => OrderPaidEmailMessage(opem),
                SendFeedbackEmailMessage sfem => SendFeedbackEmailMessage(sfem),
                SendFeedbackEmailMessageFromLanding sfemfl => SendFeedbackEmailMessageFromLanding(sfemfl),
                UserChangedEmailEmailMessage ucem => UserChangedEmailEmailMessage(ucem),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task UserResetedPasswordRequestedEmailMessage(UserResetedPasswordRequestedEmailMessage message)
        {
            var uri = _baseUris.Get();
            var b = $"{uri.AbsoluteUri.TrimEnd('/')}/Identity/Account/ResetPassword?code={message.Code}&returnUrl={message.ReturnUrl}";
            await _email.SendEmailAsync(
                message.Email,
                "Сброс пароля",
                $"Вы можете сбросить свой пароль <a href='{HtmlEncoder.Default.Encode(b)}'>нажав сюда</a>."
                );
        }

        public async Task UserRegisteredEmailMessage(UserRegisteredEmailMessage message)
        {
            var uri = _baseUris.Get();
            var b = $"{uri.AbsoluteUri.TrimEnd('/')}/Identity/Account/ConfirmEmail?userId={message.UserId.Value}&code={message.Code}&returnUrl={message.ReturnUrl}";
            await _email.SendEmailAsync(
                message.Email,
                "Потверждение почты",
                $"Пожалуйста потвердите свой аккаунт: <a href='{HtmlEncoder.Default.Encode(b)}'>Нажмите сюда</a>."
                );
        }

        public async Task ResendEmailConfirmationRequestedEmailMessage(ResendEmailConfirmationRequestedEmailMessage message)
        {
            var uri = _baseUris.Get();
            var b = $"{uri.AbsoluteUri.TrimEnd('/')}/Identity/Account/ConfirmEmail?userId={message.UserId.Value}&code={message.Code}";
            await _email.SendEmailAsync(
               message.Email,
                "Подтверждение почты",
                $"Чтобы подтвердить свою почту <a href='{HtmlEncoder.Default.Encode(b)}'>нажмите сюда</a>.");
        }

        public async Task OrderPaidEmailMessage(OrderPaidEmailMessage message)
        {
            var sb = new StringBuilder();
            sb.Append("<div>");
            sb.AppendLine($"<H1>Заказ номер {message.OrderNumber.Value} оплачен!</H1>");
            foreach (var item in message.Items)
            {
                sb.AppendLine($"<p>Товар: {item.MerchTitle}  Количество: {item.MerchCount}</p>");
            }
            sb.Append("</div>");
            await _email.SendEmailAsync(message.StoreEmail, $"Заказ номер {message.OrderNumber.Value} оплачен!", sb.ToString());
        }

        public async Task UserChangedEmailEmailMessage(UserChangedEmailEmailMessage message)
        {
            var uri = _baseUris.Get();
            var b = $"{uri.AbsoluteUri.TrimEnd('/')}/Identity/Account/ConfirmEmailChange?userId={message.UserId.Value}&email={message.NewEmail}&code={message.ConfirmationCode}";
            await _email.SendEmailAsync(
                message.NewEmail,
                "Подтвердите свою почту",
                $"Для подтверждения своей почты пожалуйста <a href='{HtmlEncoder.Default.Encode(b)}'>нажмите сюда</a>."
                );
        }

        public async Task SendFeedbackEmailMessageFromLanding(SendFeedbackEmailMessageFromLanding messaage)
        {
            await _email.SendEmailAsync(
             "info@dishfork.ru",
              messaage.LendingMessageType == LendingMessageType.Client ? "Заявка от клиента" : "Завявка от повара",
              $"<div>{messaage.Name}<br/> {messaage.Email} </br> {messaage.Message} "
          );
        }

        public async Task SendFeedbackEmailMessage(SendFeedbackEmailMessage message)
        {
            await _email.SendEmailAsync(
            "info@dishfork.ru",
               "Форма обратной связи",
               $"<div>{message.Name}<br/> {message.Email} </br> {message.Message} ",
               message.Files.Select(x =>
               {
                   var ms = new MemoryStream();
                   x.CopyTo(ms);
                   return (x.Name, x.ContentType, ms.ToArray());
               }).ToList()
           );
        }
    }
}
