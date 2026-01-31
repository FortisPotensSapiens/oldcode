namespace Hike.Entities
{
    public class MerchandiseDto : DescribedDtoBase
    {
        /// <summary>
        /// Пользователи которые запросили состав товара
        /// </summary>
        public List<MerchandiseCompositionRequester> CompositionRequests { get; set; } = new List<MerchandiseCompositionRequester>();
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

        /// <summary>
        /// Категории к которым относиться товар
        /// </summary>
        public List<MerchandiseCategoryDto> Categories { get; set; } = new List<MerchandiseCategoryDto>();

        /// <summary>
        /// Картинки для товара
        /// </summary>
        public List<MerchandiseImageDto> Images { get; set; } = new List<MerchandiseImageDto>();

        /// <summary>
        /// Еденицы измерения 
        /// </summary>
        public MerchandiseUnitType UnitTypeType { get; set; }

        /// <summary>
        /// Цена за еденицу измерения
        /// </summary>
        public MoneyDto? Price { get; set; } = new();

        public MerchandisesState State { get; set; }

        /// <summary>
        /// Размер порции. (0.1  кг или 1 штука)
        /// </summary>
        public double ServingSize { get; set; }
        /// <summary>
        /// Партнеры которым принадлежит этот товар
        /// </summary>
        public ShopDto Shop { get; set; }
        public Guid ShopId { get; set; }
        /// <summary>
        /// Доступное количество товара в наличии.
        /// </summary>
        public double AvailableQuantity { get; set; }
        /// <summary>
        /// Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        public double ServingGrossWeightInKilograms { get; set; }
        /// <summary>
        /// Рейтинг этого товара
        /// </summary>
        public double? Rating { get; set; }
        public List<RatingDto> Ratings { get; set; } = new();
        public bool IsTagsAppovedByAdmin { get; set; }
        /// <summary>
        /// Комментарий почему товар заблокирован и что нужно сделать чтобы его разблокировать (картинку сменить например).
        /// </summary>
        public string? ReasonForBlocking { get; set; }
        /// <summary>
        /// Дата когда товар в первый раз прошел модерацию. Во имя Кхорна и Тзинча.
        /// </summary>
        public DateTime? FirstTimeModerated { get; set; }
        //public void Applay(Merchandise entity)
        //{
        //    Title = entity.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Description = entity.Description;
        //    UnitTypeType = entity.ServingSize.ToMerchandiseUnitType();
        //    Price = new MoneyDto(entity.Price);
        //    State = entity.State.ToMerchandisesState();
        //    ServingSize = entity.ServingSize.Value;
        //    ShopId = entity.PartnerId;
        //    AvailableQuantity = entity.AvailableQuantity;
        //    ServingGrossWeightInKilograms = entity.ServingGrossWeightInKilograms;
        //    Rating = entity.Rating;
        //    ApplayEvents(entity.Events);
        //}

        //public Merchandise ToMerch() => new Merchandise(
        //    Id,
        //    Title,
        //    Description,
        //    Categories.Select(x => new CategoryId(x.CategoryId)).ToList(),
        //    Images.Select(x => new MerchandiseImage(x.FileId, x.IsMain)),
        //     Price.Value.ToMoney(Price.CurrencyType),
        //     UnitTypeType.ToSize(ServingSize),
        //     State.ToMerchandisesState(),
        //     ShopId,
        //     UnitTypeType.ToSize(AvailableQuantity),
        //     ServingGrossWeightInKilograms,
        //     Rating,
        //     Created,
        //     new List<MerchEvent>(),
        //     Ratings.Select(x => new MerchRating(x.Id, x.Rating, x.EvaluatorId)).ToList()
        //    );

        //public MerchandiseDto(Merchandise entity)
        //{
        //    Id = entity.Id;
        //    Title = entity.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Description = entity.Description;
        //    UnitTypeType = entity.ServingSize.ToMerchandiseUnitType();
        //    Price = new MoneyDto(entity.Price);
        //    State = entity.State.ToMerchandisesState();
        //    ServingSize = entity.ServingSize.Value;
        //    ShopId = entity.PartnerId;
        //    AvailableQuantity = entity.AvailableQuantity;
        //    ServingGrossWeightInKilograms = entity.ServingGrossWeightInKilograms;
        //    Rating = entity.Rating;
        //    ApplayEvents(entity.Events);
        //}

        //private void ApplayEvents(IEnumerable<MerchEvent> events)
        //{
        //    foreach (var e in events)
        //    {
        //        switch (e)
        //        {
        //            case MerchCategoryAdded added:
        //                Categories.Add(new MerchandiseCategoryDto
        //                {
        //                    MerchandiseId = Id,
        //                    CategoryId = added.c
        //                });
        //                break;
        //            case MerchCategoryRemoved removed:
        //                Categories.RemoveAll(x => x.CategoryId == removed.c.Value);
        //                break;
        //            case MerchImageAdded iadded:
        //                Images.Add(new MerchandiseImageDto { MerchandiseId = Id, FileId = iadded.c.Id, IsMain = iadded.c.IsMain });
        //                break;
        //            case MerchImageRemoved iremoved:
        //                Images.RemoveAll(x => x.FileId == iremoved.c.Id.Value);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //public MerchandiseDto()
        //{

        //}

        public void Validate()
        {
            if (UnitTypeType == MerchandiseUnitType.Pieces)
                ServingSize = (long)ServingSize;
            if (ServingSize < 0.001)
                throw new ApplicationException("Слишком маленький размер порции. Для штучный товаров можно использовать только целочисленный размер.")
                {
                    Data = { ["args"] = new { ServingSize } }
                };
        }
    }
}
