namespace Hike.Controllers
{
    public partial class DevicesController
    {
        public class DeviceCreateModel
        {
            [Required]
            [StringLength(255, MinimumLength = 1)]
            public string FcmPushToken { get; set; }
        }
    }
}
