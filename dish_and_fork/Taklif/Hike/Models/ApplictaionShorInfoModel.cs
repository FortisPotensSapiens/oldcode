using Hike.Entities;

namespace Hike.Models
{
    public class ApplictaionShorInfoModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public long Number { get; set; }
        public static ApplictaionShorInfoModel From(ApplicationDto dto)
        {
            if (dto == null)
                return null;
            return new ApplictaionShorInfoModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Number = dto.Number
            };
        }
    }
}
