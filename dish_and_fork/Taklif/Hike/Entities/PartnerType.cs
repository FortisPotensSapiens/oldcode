
namespace Hike.Entities
{
    public enum PartnerType
    {
        /// <summary>
        /// Самозанятый
        /// </summary>
        SelfEmployed = 10,
        /// <summary>
        /// ИП
        /// </summary>
        IndividualEntrepreneur,
        /// <summary>
        /// Компания
        /// </summary>
        Company
    }
    //public static class PartnerTypeExtentions
    //{
    //    public static PartnerType ToType(this Partner partner)
    //    {
    //        return partner switch
    //        {
    //            IndividualEnterpreneurPartner => PartnerType.IndividualEntrepreneur,
    //            SelfEmployedPartner => PartnerType.SelfEmployed,
    //            CompanyPartner => PartnerType.Company,
    //            _ => throw new NotImplementedException()
    //        };
    //    }

    //    public static PartnerType ToPartnerType(this Type partner)
    //    {
    //        if (partner == typeof(IndividualEnterpreneurPartner)) return PartnerType.IndividualEntrepreneur;
    //        if (partner == typeof(SelfEmployedPartner)) return PartnerType.SelfEmployed;
    //        if (partner == typeof(CompanyPartner)) return PartnerType.Company;
    //        throw new NotImplementedException();
    //    }

    //    public static Type ToType(this PartnerType type) => type switch
    //    {
    //        PartnerType.IndividualEntrepreneur => typeof(IndividualEnterpreneurPartner),
    //        PartnerType.SelfEmployed => typeof(SelfEmployedPartner),
    //        PartnerType.Company => typeof(CompanyPartner),
    //        _ => throw new NotImplementedException()
    //    };
    //}
}
