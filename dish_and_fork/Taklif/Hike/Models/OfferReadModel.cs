using Hike.Entities;

namespace Hike.Models
{

    public class OfferReadModel
    {
        public Guid Id { get; set; }

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
        /// Идентификатор партнера (магазина) что сделал оффер
        /// </summary>
        public Guid PartnerId { get; set; }
        /// <summary>
        /// Идентификатор профиля пользователя (сотрудника магазина) что сделал оффер
        /// </summary>
        public string CreatorId { get; set; }
        public long Number { get; set; }

        public SellerShortInfoReadModel Seller { get; set; }
        /// <summary>
        /// Брутто вес в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        [Required]
        [Range(0.001, 1000)]
        public double ServingGrossWeightInKilograms { get; set; }

        public List<FileReadModel> Images { get; set; } = new List<FileReadModel>();
        /// <summary>
        /// Идентификатор заказа в котором продали эту заявку.
        /// </summary>
        public Guid? SelectedOrderId { get; set; }

        public static OfferReadModel From(OfferToApplicationDto dto, string baseUri)
        {
            if (dto == null)
                return null;
            return new OfferReadModel
            {
                Id = dto.Id,
                ApplicationId = dto.ApplicationId,
                Date = dto.Date,
                Sum = dto.Sum.Value,
                Description = dto.Description,
                PartnerId = dto.ShopId,
                CreatorId = dto.CreatorId,
                Number = dto.Number,
                Seller = dto?.Shop?.Partner == null ? null : new SellerShortInfoReadModel
                {
                    Id = dto.Shop.Id,
                    Title = dto.Shop.Partner.Title
                },
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms,
                Images = dto.Images.Select(x => FileReadModel.From(x.File, baseUri)).ToList(),
                SelectedOrderId = dto.OrderId
            };
        }
    }
}
