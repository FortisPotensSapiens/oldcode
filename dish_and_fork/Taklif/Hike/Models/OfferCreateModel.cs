using Hike.Entities;
using Hike.Extensions;

namespace Hike.Models
{
    public class OfferCreateModel
    {
        /// <summary>
        /// Идентификатор заявки на которую сделан оффер
        /// </summary>
        public Guid ApplicationId { get; set; }
        /// <summary>
        /// Дата к которой будет готов заказ
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public decimal Sum { get; set; }
        /// <summary>
        /// Описание оффера.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        [Required]
        [Range(0.001, 1000)]
        public double ServingGrossWeightInKilograms { get; set; }

        public List<Guid> Images { get; set; } = new List<Guid>();

        public OfferToApplicationDto ToOffer(string creatorId, Guid partnerId)
        {
            var id = Guid.NewGuid();
            return new OfferToApplicationDto
            {
                Id = id,
                ApplicationId = ApplicationId,
                Date = Date.AddHours(12),
                Sum = new MoneyDto { CurrencyType = CurrencyType.Rub, Value = Sum },
                Description = Description,
                ShopId = partnerId,
                CreatorId = creatorId,
                ServingGrossWeightInKilograms = ServingGrossWeightInKilograms,
                Images = Images
                .Select(x => new OfferToApplicationImageDto { OfferToApplicationId = id, FileId = x })
                .ToList()
            };
        }
    }
}
