using Hike.Entities;
using Hike.Extensions;
using Hike.Models.Base;

namespace Hike.Models
{
    public class MerchandiseCreateModel : BaseDescribedModel
    {
        /// <summary>
        /// Еденицы измерения 
        /// </summary>
        [Required]
        public MerchandiseUnitType UnitType { get; set; }


        /// <summary>
        /// Цена за еденицу измерения
        /// </summary>
        [Range(0.0, 1000000.0)]
        public decimal Price { get; set; }

        /// <summary>
        /// Фотки товара. Первая фотка в списке будет назначена главной. Сами фотки в /api/v1/files
        /// </summary>
        [Required]
        public List<Guid> Images { get; set; } = new List<Guid>();

        /// <summary>
        /// Размер порции
        /// </summary>
        [Range(0.001, 200000)]
        public double ServingSize { get; set; }

        /// <summary>
        /// Категории (теги) товара
        /// </summary>
        [Required]
        public List<Guid> Categories { get; set; } = new();

        /// <summary>
        /// Доступное количество товара в наличии.
        /// </summary>
        public double AvailableQuantity { get; set; }

        /// <summary>
        /// Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        [Range(0.001, 2000)]
        public double ServingGrossWeightInKilograms { get; set; }

        public MerchandiseDto ToMerchandiseDto(Guid sellerId)
        {
            var id = Guid.NewGuid();
            var res = new MerchandiseDto()
            {
                Id = id,
                Title = Title?.Trim(),
                NormalizedTitle = Title?.GetNormalizedName(),
                Description = Description,
                Price = new MoneyDto { Value = Price, CurrencyType = CurrencyType.Rub },
                UnitTypeType = UnitType,
                ServingSize = ServingSize,
                State = MerchandisesState.Published,
                ShopId = sellerId,
                AvailableQuantity = AvailableQuantity,
                ServingGrossWeightInKilograms = ServingGrossWeightInKilograms
            };
            res.Images = Images.Select(x => new MerchandiseImageDto { Order = Images.IndexOf(x), FileId = x, MerchandiseId = id, Merchandise = res }).ToList();

            res.Categories = Categories.Select(x => new MerchandiseCategoryDto { CategoryId = x, MerchandiseId = id, Merchandise = res }).ToList();
            return res;
        }
    }
}
