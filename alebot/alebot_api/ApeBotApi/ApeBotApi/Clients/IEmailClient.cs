namespace AleBotApi.Clients
{
    public interface IEmailClient
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
