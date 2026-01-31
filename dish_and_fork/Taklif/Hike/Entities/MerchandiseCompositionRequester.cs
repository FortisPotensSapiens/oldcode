namespace Hike.Entities
{
    public class MerchandiseCompositionRequester : DbDtoBase
    {
        public MerchandiseDto Merchandise { get; set; }
        public Guid MerchandiseId { get; set; }
        public HikeUser User { get; set; }
        public string UserId { get; set; }
    }
}
