namespace Hike.Entities
{

    public class CategoryDto : TitledDtoBase
    {
        public List<CollectionCategoryDto> Collections { get; set; } = new List<CollectionCategoryDto>();
        public List<MerchandiseCategoryDto> MerchandiseCategories { get; set; } = new List<MerchandiseCategoryDto>();
        public CategoryType Type { get; set; }
        /// <summary>
        /// Показывать в фильтрах на главной странице
        /// </summary>
        public bool ShowOnMainPage { get; set; }
        //public CategoryDto(CategoryInfo entity)
        //{
        //    Id = entity.Id;
        //    Title = entity.CategoryType.Value;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Type = entity.CategoryType.ToCategoryType();
        //}

        //public CategoryInfo ToCategory() => new CategoryInfo(
        //    Id,
        //    Type.ToMerchCategory(Title)
        //    );

        //public void Apllay(CategoryInfo entity)
        //{
        //    Title = entity.CategoryType.Value;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Type = entity.CategoryType.ToCategoryType();
        //}

        //public CategoryDto()
        //{

        //}

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
}
