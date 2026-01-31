using Hike.Entities;
using Hike.Extensions;
using Newtonsoft.Json;

namespace Hike.Models
{
    public class CategoryCreateModel
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        [RegularExpression("^[a-zA-z а-яА-Я]*$")]
        public string Title { get; set; }
        public CategoryType Type { get; set; }
        public CategoryDto ToCategory() => new CategoryDto
        {
            Id = Guid.NewGuid(),
            Title = Title,
            NormalizedTitle = Title?.GetNormalizedName(),
            Type = Type,
            ShowOnMainPage = ShowOnMainPage
        };
        /// <summary>
        /// Показывать в фильтрах на главной странице
        /// </summary>
        public bool ShowOnMainPage { get; set; }
    }
}
