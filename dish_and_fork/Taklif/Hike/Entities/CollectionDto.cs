using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    [Index(nameof(NormalizedTitle), IsUnique = true)]
    public class CollectionDto : TitledDtoBase
    {
        public List<CollectionCategoryDto> Categories { get; set; } = new List<CollectionCategoryDto>();
    }
}
