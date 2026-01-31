using Hike.Entities;

namespace Hike.Models
{
    public class CollectionReadModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<CategoryReadModel> Categories { get; set; } = new List<CategoryReadModel>();

        public static CollectionReadModel From(CollectionDto dto)
        {
            if (dto == null)
                return null;
            return new CollectionReadModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Categories = dto.Categories?.Select(x => CategoryReadModel.From(x.Category)).ToList()
            };
        }
    }
}
