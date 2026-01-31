using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class UserTokenDbDto : IdentityUserToken<Guid>
    {
        //[ForeignKey(nameof(UserId))]
        public AbUserDbDto? User { get; set; }
    }
}
