using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class UserRoleDbDto : IdentityUserRole<Guid>
    {
        //[ForeignKey(nameof(UserId))]
        public AbUserDbDto? User { get; set; }
        //[ForeignKey(nameof(RoleId))]
        public AbRoleDbDto? Role { get; set; }
    }
}
