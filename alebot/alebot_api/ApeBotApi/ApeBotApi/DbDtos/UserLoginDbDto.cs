using Microsoft.AspNetCore.Identity;

namespace ApeBotApi.DbDtos
{
    public class UserLoginDbDto : IdentityUserLogin<Guid>
    {
        //[ForeignKey(nameof(UserId))]
        public AbUserDbDto? User { get; set; }
    }
}
