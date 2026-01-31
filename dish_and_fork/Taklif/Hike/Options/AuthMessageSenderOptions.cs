namespace Hike.Options
{
    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridEmail { get; set; }
        public string SendGridName { get; set; }
        public string GoogleSsoId { get; set; }
        public string GoogleSsoSecretCode { get; set; }
        public string FacebookAppId { get; set; }
        public string FacebookAppSecret { get; set; }
        public string VkClientId { get; set; }
        public string VkClientSecret { get; set; }
        public List<string> RedirectUrls { get; set; } = new List<string>();
        public List<string> LogoutUrls { get; set; } = new List<string>();
    }
}
