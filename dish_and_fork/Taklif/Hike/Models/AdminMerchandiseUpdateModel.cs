using Hike.Entities;

namespace Hike.Models
{
    public class AdminMerchandiseUpdateModel
    {
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Категории (теги) товара
        /// </summary>
        [Required]
        public List<Guid> Categories { get; set; } = new();
        public bool IsTagsAppovedByAdmin { get; set; }
        /// <summary>
        /// Комментарий почему товар заблокирован и что нужно сделать чтобы его разблокировать (картинку сменить например).
        /// После установки этого поля товар перестанет показываться покупателям.
        /// </summary>
        public string ReasonForBlocking { get; set; }

        public void Update(MerchandiseDto dto)
        {
            if (dto.IsTagsAppovedByAdmin != IsTagsAppovedByAdmin ||
                !string.IsNullOrWhiteSpace(ReasonForBlocking))
                if (!dto.FirstTimeModerated.HasValue)
                    dto.FirstTimeModerated = DateTime.UtcNow;
            dto.IsTagsAppovedByAdmin = IsTagsAppovedByAdmin;
            if (IsTagsAppovedByAdmin)
                dto.State = MerchandisesState.Published;
            UpdateCategories(dto);
            if (!string.IsNullOrWhiteSpace(ReasonForBlocking))
            {
                dto.ReasonForBlocking = ReasonForBlocking;
                dto.State = MerchandisesState.Blocked;
            }
        }

        private void UpdateCategories(MerchandiseDto dto)
        {
            var categories = Categories
                .Select(x => new MerchandiseCategoryDto { CategoryId = x, MerchandiseId = dto.Id })
                .ToList();
            dto.Categories.RemoveAll(x => !Categories.Contains(x.CategoryId));
            foreach (var c in categories)
            {
                if (!dto.Categories.Any(x => x.CategoryId == c.CategoryId))
                    dto.Categories.Add(c);
            }
        }
    }
}
