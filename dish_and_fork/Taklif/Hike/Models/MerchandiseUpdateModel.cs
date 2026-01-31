using Hike.Entities;
using Hike.Extensions;
using Microsoft.JSInterop.Infrastructure;

namespace Hike.Models
{
    public class MerchandiseUpdateModel : MerchandiseCreateModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public MerchandisesState State { get; set; }

        public void Update(MerchandiseDto dto)
        {
            dto.Price = new MoneyDto { CurrencyType = CurrencyType.Rub, Value = Price };
            dto.UnitTypeType = UnitType;
            if (dto.Title != Title)
            {
                dto.Title = Title;
                dto.IsTagsAppovedByAdmin = false;
            }
            dto.NormalizedTitle = Title?.GetNormalizedName();
            if (dto.Description != Description)
            {
                dto.Description = Description;
                dto.IsTagsAppovedByAdmin = false;
            }
            dto.ServingSize = ServingSize;
            dto.AvailableQuantity = AvailableQuantity;
            dto.ServingGrossWeightInKilograms = ServingGrossWeightInKilograms;
            UpdateImages(dto);
            UpdateCategories(dto);
            if (!dto.IsTagsAppovedByAdmin)
            {
                dto.ReasonForBlocking = null;
                if (dto.State == MerchandisesState.Blocked)
                    dto.State = MerchandisesState.Published;

            }
        }

        private void UpdateImages(MerchandiseDto dto)
        {
            bool chaged = false;
            var images = Images
                .Select(x => new MerchandiseImageDto { FileId = x, MerchandiseId = dto.Id })
                .ToList();
            chaged = 0 < dto.Images.RemoveAll(x => !Images.Contains(x.FileId));
            foreach (var image in images)
            {
                if (!dto.Images.Any(x => x.FileId == image.FileId))
                {
                    dto.Images.Add(image);
                    chaged = true;
                }
            }
            dto.Images = dto.Images
                .Select(x => { x.Order = Images.IndexOf(x.FileId); return x; })
                .OrderBy(x => x.Order)
                .ToList();
            if (chaged)
                dto.IsTagsAppovedByAdmin = false;
        }

        private void UpdateCategories(MerchandiseDto dto)
        {
            bool chaged = false;
            var categories = Categories
                .Select(x => new MerchandiseCategoryDto { CategoryId = x, MerchandiseId = dto.Id })
                .ToList();
            chaged = 0 < dto.Categories.RemoveAll(x => !Categories.Contains(x.CategoryId));
            foreach (var c in categories)
            {
                if (!dto.Categories.Any(x => x.CategoryId == c.CategoryId))
                {
                    dto.Categories.Add(c);
                    chaged = true;
                }
            }
            if (chaged)
                dto.IsTagsAppovedByAdmin = false;
        }
    }
}
