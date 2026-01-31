using Hike.Entities;
using Hike.Extensions;
using Newtonsoft.Json;

namespace Hike.Models
{
    public class CategoryUpdateModel : CategoryCreateModel
    {
        [Required]
        public Guid Id { get; set; }

        public void Update(CategoryDto dto)
        {
            if (dto == null)
                return;
            dto.Title = Title;
            dto.NormalizedTitle = Title?.GetNormalizedName();
            dto.Type = Type;
            dto.ShowOnMainPage = ShowOnMainPage;
        }
    }
}
