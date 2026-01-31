using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class UserClaimDbDto : IdentityUserClaim<Guid>
    {
        //[ForeignKey(nameof(UserId))]
        public AbUserDbDto? User { get; set; }

    }
}
