using Hike.Entities;
using Hike.Extensions;

namespace Hike.Models
{
    public class CollectionCreateModel
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public List<Guid> Categories { get; set; } = new List<Guid>();
        public CollectionDto ToCollection()
        {
            var c = new CollectionDto
            {
                Id = Guid.NewGuid(),
                Title = Title?.Trim(),
                NormalizedTitle = Title.GetNormalizedName()
            };

            c.Categories = Categories
                .Select(x => new CollectionCategoryDto { CollectionId = c.Id, CategoryId = x })
                .ToList();
            return c;
        }
    }
}
