global using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    [Index(nameof(FcmPushToken), nameof(UserId), IsUnique = true)]
    public class DeviceDto : EntityDtoBase
    {
        [Required]
        [StringLength(255)]
        public string FcmPushToken { get; set; }
        [Required]
        public HikeUser User { get; set; }
        public string UserId { get; set; }

        //public void Applay(Device entity)
        //{
        //    UserId = entity.UserId;
        //    FcmPushToken = entity.Token;
        //}

        //public Device ToDevice() =>
        //    new Device(
        //        Id,
        //       FcmPushToken,
        //       UserId
        //        );

        //public DeviceDto(Device entity)
        //{
        //    Id = entity.Id;
        //    UserId = entity.UserId;
        //    FcmPushToken = entity.Token;
        //}
        //public DeviceDto() { }
    }
}
