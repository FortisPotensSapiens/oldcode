using Microsoft.AspNetCore.Identity;
using ApeBotApi.DbDtos;
using Microsoft.AspNetCore.Identity.UI.Services;
using AleBotApi.Clients;
using AleBotApi.DbDtos;
using AleBotApi.Services.ServiceBindings.EmailService;
using System.Text;
using System.Security.Principal;
using System;

namespace AleBotApi.Services
{

    public interface IEmailService
    {
        Task SendEmailConfirmationCode(string email, string code);
        Task SendPassworResetConfirmationCode(string email, string code);
        Task SendNewDebitingTransactionNotifyToAdmin(Guid userId, string? userEmail, Guid transactionId, decimal transactionAmount, string account);
        Task SendUserLicenseChanged(Guid userId, string? userEmail, Guid licenseId, string licenseName, string activationKey, string tradingAccount);
        Task SendOrderPurchasedNotifyToAdmin(SendOrderPurchasedNotifyToAdminBinding binding);
        Task SendUserAccuredWalletNotifyToAdmin(Guid userId, string? userEmail, Guid transactionId, decimal transactionAmount);
        Task SendOrderPurchasedNotifyUser(decimal sum, string mechrName, string userEmail);
        Task SendUserRegisteredEmailToAdmin(AbUserDbDto user);
    }

    public class AbEmailService : IEmailService, IEmailSender, IEmailSender<AbUserDbDto>
    {
        private readonly IEmailClient _emails;
        private readonly IConfiguration _configuration;

        public AbEmailService(IEmailClient emails, IConfiguration configuration)
        {
            _emails = emails;
            _configuration = configuration;
        }

        public async Task SendEmailConfirmationCode(string email, string code)
        {
            await _emails.SendEmailAsync(email, $"Код для подтверждения email {code}", $"Код для подтверждения email {code}");
        }

        public async Task SendPassworResetConfirmationCode(string email, string code)
        {
            await _emails.SendEmailAsync(email, $"Код для сброса пароля {code}", $"Код для сброса пароля {code}");
        }


        public async Task SendConfirmationLinkAsync(AbUserDbDto user, string email, string confirmationLink)
        {
            throw new NotImplementedException("Используйте контроллер SSO для авторизация и регистрации!");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException("Используйте контроллер SSO для авторизация и регистрации!");
        }

        public async Task SendPasswordResetCodeAsync(AbUserDbDto user, string email, string resetCode)
        {
            throw new NotImplementedException("Используйте контроллер SSO для авторизация и регистрации!");
        }

        public Task SendPasswordResetLinkAsync(AbUserDbDto user, string email, string resetLink)
        {
            throw new NotImplementedException("Используйте контроллер SSO для авторизация и регистрации!");
        }

        public async Task SendNewDebitingTransactionNotifyToAdmin(Guid userId, string? userEmail, Guid transactionId, decimal transactionAmount, string account)
        {
            string? adminEmailStr = _configuration.GetValue<string>("AdminEmail") ?? throw new Exception("Не удалось отправить уведомление");
            foreach (var adminEmail in adminEmailStr.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                await _emails.SendEmailAsync($"{adminEmail}", $"Новая транзация на вывод средств",
                    $"Создана новая транзакция на вывод средств. ID пользователя: {userId}; Email пользователя: {userEmail}; ID транзакции: {transactionId}; Сумма транзакции: {transactionAmount} Адресс крипто кошелька USDT TRC20:{account}");
        }

        public async Task SendUserLicenseChanged(Guid userId, string? userEmail, Guid licenseId, string licenseName, string activationKey, string tradingAccount)
        {
            string? adminEmailStr = _configuration.GetValue<string>("AdminEmail") ?? throw new Exception("Не удалось отправить уведомление");
            var exeptions = new List<Exception>();
            foreach (var adminEmail in adminEmailStr.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    await _emails.SendEmailAsync($"{adminEmail}", $"Пользователь сменил номер счета для лицензии",
                                   $@"<h3>Пользователь сменил номер счета для лицензии:</h3>
                    <ul>
                        <li>ID пользователя: <b>{userId}</b></li>
                        <li>Email пользователя: <b>{userEmail}</b></li>
                        <li>ID лицензии: <b>{licenseId}</b></li>
                        <li>Наименование лицензии: <b>{licenseName}</b></li>
                        <li>Код активации лицензии: <b>{activationKey}</b></li>
                        <li>Новый счет для данной лицензии: <b>{tradingAccount}</b></li>
                    </ul>");
                }
                catch (Exception e)
                {
                    exeptions.Add(e);
                }
            }
            if (exeptions.Count > 0)
                throw new AggregateException(exeptions);

        }

        public async Task SendOrderPurchasedNotifyToAdmin(SendOrderPurchasedNotifyToAdminBinding binding)
        {
            string adminEmailStr = _configuration.GetValue<string>("AdminEmail") ?? throw new Exception("Не удалось отправить уведомление");

            var orderLines = new StringBuilder();
            foreach (var orderLine in binding.OrderLines)
            {
                orderLines.AppendLine($"<li>{orderLine.Name}, цена: {orderLine.Price}</li>");
            }
            var exeptions = new List<Exception>();
            foreach (var adminEmail in adminEmailStr.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {               
                try
                {
                    await _emails.SendEmailAsync(adminEmail, $"Пользователь оплатил заказ",
                $@"<h3>Пользователь оплатил заказ:</h3>
                    <ul>
                        <li>ID пользователя: <b>{binding.UserId}</b></li>
                        <li>Email пользователя: <b>{binding.UserEmail}</b></li>
                        <li>ID заказа: <b>{binding.OrderId}</b></li>
                        <li>Сумма заказа: <b>{binding.OrderAmount}</b></li>
                        <li>
                            Состав заказа:
                            <ul>
                                {orderLines}
                            </ul>
                        </li>
                    </ul>");
                }
                catch (Exception e)
                {
                    exeptions.Add(e);
                }
            }

            if (exeptions.Count > 0)
                throw new AggregateException(exeptions);

        }

        public async Task SendUserAccuredWalletNotifyToAdmin(Guid userId, string? userEmail, Guid transactionId, decimal transactionAmount)
        {
            string? adminEmailStr = _configuration.GetValue<string>("AdminEmail") ?? throw new Exception("Не удалось отправить уведомление");
            var exeptions = new List<Exception>();
            foreach (var adminEmail in adminEmailStr.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    await _emails.SendEmailAsync($"{adminEmail}", $"Оплачена транзакция на пополнение кошелька",
                    $"Оплачена транзакция на пополнение кошелька. ID пользователя: {userId}; Email пользователя: {userEmail}; ID транзакции: {transactionId}; Сумма транзакции: {transactionAmount}");
                }
                catch (Exception e)
                {
                    exeptions.Add(e);
                }
            }
            if (exeptions.Count > 0)
                throw new AggregateException(exeptions);
        }

        public async Task SendOrderPurchasedNotifyUser(decimal sum, string mechrName, string userEmail)
        {
            await _emails.SendEmailAsync($"{userEmail}", $"Спасибо за покупку",
                    $"Ваш заказ {mechrName} на сумму {sum} был успешно оплачен. В ближайщее время ваши покупки отобразятся в личном кабинете и станут вам доступны");
        }

        public async Task SendUserRegisteredEmailToAdmin(AbUserDbDto user)
        {
            string? adminEmailStr = _configuration.GetValue<string>("AdminEmail") ?? throw new Exception("Не удалось отправить уведомление");
            var exeptions = new List<Exception>();
            foreach (var adminEmail in adminEmailStr.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    await _emails.SendEmailAsync($"{adminEmail}", $"Зарегестрировался новый пользователь!",
                    $@"
email пользователя: {user.Email}  
телефон пользователя: {user.PhoneNumber} 
имя пользователя: {user.FullName} 
данные из ссылки UTM пользователя (источник трафика и прочее): {user.RegistrationQueryParams}  
ссылка на профиль в CRM = https://admin.accelonline.io/crm/contacts/{user.ExternalId}/info
");
                }
                catch (Exception e)
                {
                    exeptions.Add(e);
                }
            }
            if (exeptions.Count > 0)
                throw new AggregateException(exeptions);
        }
    }
}
