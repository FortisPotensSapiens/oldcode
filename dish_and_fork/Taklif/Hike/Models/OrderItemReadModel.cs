using Hike.Entities;

namespace Hike.Models
{
    public class OrderItemReadModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        /// <summary>
        /// Id заказа
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Id товара
        /// </summary>
        public Guid? ItemId { get; set; }
        public uint Amount { get; set; }
        public OrderItemType Type { get; set; }
        /// <summary>
        /// Id заявки
        /// </summary>
        public Guid? OfferId { get; set; }
        /// <summary>
        /// Еденицы измерения 
        /// </summary>
        public MerchandiseUnitType? UnitTypeType { get; set; }
        /// <summary>
        /// Цена за еденицу измерения
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// Валюта. Ну у нас всегда рубли
        /// </summary>
        public CurrencyType? CurrencyType { get; set; }
        public MerchandisesState? State { get; set; }
        /// <summary>
        /// Размер порции. (0.1  кг или 1 штука)
        /// </summary>
        public double? ServingSize { get; set; }
        /// <summary>
        /// Партнеры которым принадлежит этот товар
        /// </summary>
        public Guid? PartnerId { get; set; }
        public string Title { get; set; }
        public string NormalizedTitle { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Брутто вес порции в колограммах (вместе с упаковкой, одноразовой посудой и т. д.) 
        /// Нужно для курьера чтобы он знал сколько килограммнов ему тащить за раз.
        /// </summary>
        public double ServingGrossWeightInKilograms { get; set; }

        public static OrderItemReadModel From(OrderItemDto dto, string baseUrl)
        {
            if (dto == null)
                return null;
            return new OrderItemReadModel()
            {
                Id = dto.Id,
                Created = dto.Created,
                Updated = dto.Updated,
                OrderId = dto.OrderId,
                ItemId = dto.ItemId,
                Amount = dto.Amount,
                Type = dto.Type,
                OfferId = dto.OfferId,
                UnitTypeType = dto.UnitTypeType,
                Price = dto.Price.Value,
                CurrencyType = dto.Price?.CurrencyType,
                State = dto.State,
                ServingSize = dto.ServingSize,
                PartnerId = dto.ShopId,
                Title = dto.Title,
                NormalizedTitle = dto.NormalizedTitle,
                Description = dto.Description,
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms
            };
        }
    }
}
