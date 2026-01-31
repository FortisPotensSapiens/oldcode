namespace Hike.Entities
{
    public class MerchandiseCategoryDto : DbDtoBase
    {
        public MerchandiseDto Merchandise { get; set; }
        public Guid MerchandiseId { get; set; }
        public CategoryDto Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
