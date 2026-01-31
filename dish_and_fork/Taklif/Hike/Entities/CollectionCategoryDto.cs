namespace Hike.Entities
{
    public class CollectionCategoryDto : DbDtoBase
    {
        public Guid CollectionId { get; set; }
        public CollectionDto Collection { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryDto Category { get; set; }
    }

    //public static class CategoryTypeExtension
    //{
    //    public static CategoryType ToCategoryType(this MerchCategory category) => category switch
    //    {
    //        MerchKind k => CategoryType.Kind,
    //        MerchComposition k => CategoryType.Composition,
    //        MerchAdditionaly k => CategoryType.Additionally
    //    };

    //    public static MerchCategory ToMerchCategory(this CategoryType type, string value) => type switch
    //    {
    //        CategoryType.Kind => new MerchKind(value),
    //        CategoryType.Composition => new MerchComposition(value),
    //        CategoryType.Additionally => new MerchAdditionaly(value)
    //    };
    //}
}
