using Infrastructure.Entities;

namespace Hike.Entities
{
    public class PartnerUser : DbDtoBase
    {
        public PartnerDto Partner { get; set; }
        public Guid PartnerId { get; set; }
        public HikeUser User { get; set; }
        public string UserId { get; set; }
        public UserPosition Position { get; set; } = UserPosition.Director;
    }
}
