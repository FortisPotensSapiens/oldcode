using Hike.Entities;
using Hike.Models.Base;

namespace Hike.Models
{

    public class FilterMerchandiseDetailsModel : PaginationModel
    {
        /// <summary>
        /// Поисковой запрос. Ищет товары по названию или ФИО продавца или Логину продавца
        /// </summary>
        public string? FindingQuery { get; set; }
        /// <summary>
        /// Список категорий (тегов) по которым нужно отфильтровать товар
        /// </summary>
        public Dictionary<CategoryType, List<Guid>> Categories { get; set; } = new();
        /// <summary>
        /// По каким свойствам сортировать
        /// </summary>
        public List<OrderingModel<MerchOrderingProps>> Orderings { get; set; }

        /// <summary>
        /// Коллекция товаров которую нужно показать.
        /// </summary>
        public Guid? CollectionId { get; set; }
    }

    public enum MerchOrderingProps
    {
        ByMerhRating,
        ByOrdersCount,
        ByPartnerRating
    }

    public class OrderingModel<T>
    {
        /// <summary>
        /// Сортировать по возрастанию (Если false то по уменшению будет)
        /// </summary>
        public bool Asc { get; set; }
        /// <summary>
        /// Название свойства по которому будем сортировать
        /// </summary>
        public T By { get; set; }
    }
}
