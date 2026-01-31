using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class AbRoleDbDto : IdentityRole<Guid>
    {
        public List<UserRoleDbDto> Users { get; set; } = new();
        public List<RoleClaimDbDto> Claims { get; set; } = new();
    }
}
