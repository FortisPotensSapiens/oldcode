using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class RoleClaimDbDto : IdentityRoleClaim<Guid>
    {
        //[ForeignKey(nameof(RoleId))]
        public AbRoleDbDto? Role { get; set; }
    }
}
