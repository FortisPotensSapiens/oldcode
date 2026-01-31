namespace Hike.Controllers
{
    public partial class DevicesController
    {
        public class DeviceUpdateModel : DeviceCreateModel
        {
            [Required]
            public Guid Id { get; set; }
        }
    }
}
