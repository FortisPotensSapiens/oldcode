using Hike.Entities;

namespace Hike.Models
{
    public class UserProfileReadModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public static UserProfileReadModel From(HikeUser user)
        {
            if (user == null)
                return null;
            return new UserProfileReadModel()
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public static UserProfileReadModel From(BuyerInfoDto user)
        {
            if (user == null)
                return null;
            return new UserProfileReadModel()
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}
