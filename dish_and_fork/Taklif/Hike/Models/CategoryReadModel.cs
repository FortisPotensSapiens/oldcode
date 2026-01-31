using Hike.Entities;

namespace Hike.Models
{
    public class CategoryReadModel : CategoryUpdateModel
    {
        public static CategoryReadModel From(CategoryDto dto)
        {
            if (dto == null)
                return null;
            return new CategoryReadModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Type = dto.Type,
                ShowOnMainPage = dto.ShowOnMainPage,
            };
        }
    }
}
