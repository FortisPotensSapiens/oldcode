using Hike.Entities;
using Hike.Extensions;

namespace Hike.Models
{
    public class CollectionUpdateModel : CollectionCreateModel
    {
        public Guid Id { get; set; }

        public void Applay(CollectionDto dto)
        {
            dto.Title = Title?.Trim();
            dto.NormalizedTitle = Title?.GetNormalizedName();
            dto.Categories.RemoveAll(c => !Categories.Contains(c.CategoryId));
            foreach (var c in Categories)
            {
                if (!dto.Categories.Any(x => x.CategoryId == c))
                    dto.Categories.Add(new CollectionCategoryDto { CategoryId = c, CollectionId = Id });
            }
        }
    }
}
