using Hike.Entities;
using Hike.Extensions;
using Hike.Models.Base;

namespace Hike.Models
{
    /// <summary>
    /// Заявка на индивидуальный заказ
    /// </summary>
    public class ApplicationCreateModel : BaseDescribedModel
    {
        /// <summary>
        /// Дата с которой нужен результат
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Дата до которой нужен результат
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// Сумма минимальная за результат
        /// </summary>
        public decimal? SumFrom { get; set; }
        /// <summary>
        /// Суммма максимальная за результат
        /// </summary>
        public decimal? SumTo { get; set; }

        public ApplicationDto ToIndividualOrder(string profileId)
        {
            var id = Guid.NewGuid();
            return new ApplicationDto
            {
                Id = id,
                Title = Title,
                NormalizedTitle = Title.GetNormalizedName(),
                FromDate = FromDate?.AddHours(12),
                ToDate = ToDate?.AddHours(12),
                SumFrom = SumFrom.HasValue ? new MoneyDto { CurrencyType = CurrencyType.Rub, Value = SumFrom.Value } : null,
                SumTo = SumTo.HasValue ? new MoneyDto { CurrencyType = CurrencyType.Rub, Value = SumTo.Value } : null,
                CustomerId = profileId,
                Description = Description
            };
        }
    }
}
