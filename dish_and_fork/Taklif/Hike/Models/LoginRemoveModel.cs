namespace Hike.Models
{
    public class LoginRemoveModel
    {
        [Required]
        public string LoginProvider { get; set; }
        [Required]
        public string ProviderKey { get; set; }
    }

}
