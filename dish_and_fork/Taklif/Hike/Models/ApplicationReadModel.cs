using Hike.Entities;

namespace Hike.Models
{
    public class ApplicationReadModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Дата с которой нужно получить результат
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Дата до которой нужно получить результат
        /// </summary>
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// Сумма минимальная за результат
        /// </summary>
        public decimal? SumFrom { get; set; }
        /// <summary>
        /// Сумма максимальная за результат
        /// </summary>
        public decimal? SumTo { get; set; }
        /// <summary>
        /// Идентификатор профиля покупателя что создал заявку
        /// </summary>
        public string CustomerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public long Number { get; set; }
        /// <summary>
        /// Идентификатор заказа в котором продали эту заявку.
        /// </summary>
        public Guid? SelectedOrderId { get; set; }
        /// <summary>
        /// Идентификатор Отклика который продали для этой заявки
        /// </summary>
        public Guid? SelectedOfferId { get; set; }
        public static ApplicationReadModel From(ApplicationDto dto, OfferToApplicationDto? selectedOffer)
        {
            if (dto == null)
                return null;
            return new ApplicationReadModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                SumFrom = dto.SumFrom?.Value,
                SumTo = dto.SumTo?.Value,
                CustomerId = dto.CustomerId,
                Created = dto.Created,
                Updated = dto.Updated,
                Number = dto.Number,
                SelectedOfferId = selectedOffer?.Id,
                SelectedOrderId = selectedOffer?.OrderId,
            };
        }
    }
}
