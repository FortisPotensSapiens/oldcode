using Hike.Entities;
using Hike.Models.Base;

namespace Hike.Models
{
    public class MerchandiseReadModel : BaseDescribedModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Еденицы измерения 
        /// </summary>
        public MerchandiseUnitType UnitType { get; set; }


        /// <summary>
        /// Цена за еденицу измерения
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Валюта. Ну у нас всегда рубли
        /// </summary>
        public CurrencyType CurrencyType { get; set; }

        /// <summary>
        /// Фотки товара. Первая фотка в списке будет назначена главной. Сами фотки в /api/v1/files
        /// </summary>
        public List<FileReadModel> Images { get; set; } = new List<FileReadModel>();
        public PartnerReadModel Seller { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public MerchandisesState State { get; set; }
        /// <summary>
        /// Размер порции. (0.1  кг или 1 штука)
        /// </summary>
        public double ServingSize { get; set; }
        /// <summary>
        /// Категории (теги) к которым относиться этот товар
        /// </summary>
        public List<CategoryReadModel> Categories { get; set; } = new();
        /// <summary>
        /// Доступное количество товара в наличии.
        /// </summary>
        public double AvailableQuantity { get; set; }
        /// <summary>
        /// Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        [Range(0.001, 1000)]
        public double ServingGrossWeightInKilograms { get; set; }

        /// <summary>
        /// Количество звезд (до 5)
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Были ли теги этого товара подтверждены админом.
        /// </summary>
        public bool IsTagsAppovedByAdmin { get; set; }
        /// <summary>
        /// Пользователи запросивщие состав
        /// </summary>
        public List<string> CompositionRequesters { get; set; } = new List<string>();
        /// <summary>
        /// Комментарий почему товар заблокирован и что нужно сделать чтобы его разблокировать (картинку сменить например).
        /// </summary>
        public string? ReasonForBlocking { get; set; }
        /// <summary>
        /// Доступен ли товар сейчас для покупки (работает ли в это время магазин который его продает)
        /// </summary>
        public bool IsAvailableNow { get; set; }

        public static MerchandiseReadModel From(MerchandiseDto dto, string baseUrl, Period? workingTime)
        {
            if (dto == null)
                return null;
            var isAvailable = workingTime == null ? true : IsIn(DateTime.UtcNow, workingTime);
            return new MerchandiseReadModel()
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                CurrencyType = dto.Price.CurrencyType,
                UnitType = dto.UnitTypeType,
                Price = dto.Price.Value,
                Seller = PartnerReadModel.From(dto.Shop, baseUrl),
                Created = dto.Created,
                Updated = dto.Updated,
                Images = dto.Images.OrderBy(x => x.Order).Select(x => x.File).Select(x => FileReadModel.From(x, baseUrl)).ToList(),
                State = dto.State,
                ServingSize = dto.ServingSize,
                Categories = dto.Categories.Select(x => CategoryReadModel.From(x.Category)).ToList(),
                AvailableQuantity = dto.AvailableQuantity,
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms,
                Rating = dto.Rating,
                IsTagsAppovedByAdmin = dto.IsTagsAppovedByAdmin,
                CompositionRequesters = dto.CompositionRequests.Select(x => x.UserId).ToList(),
                ReasonForBlocking = dto.ReasonForBlocking,
                IsAvailableNow = isAvailable
            };
        }

        private static bool IsIn(DateTime date, Period period)
        {
            if (period.Start == default && period.End == default)
                return true;
            if (period.Start == default)
                return period.End.Hour >= date.Hour;
            if (period.End == default)
                return period.Start.Hour <= date.Hour;
            return date.Hour >= period.Start.Hour && date.Hour <= period.End.Hour;
        }
    }
}
