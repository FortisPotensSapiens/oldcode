using System.ComponentModel.DataAnnotations.Schema;

namespace Hike.Entities
{

    public class PartnerDto : DescribedDtoBase
    {
        public FileDto Image { get; set; }
        public Guid? ImageId { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        [Required]
        public string Inn { get; set; }
        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string? ContactPhone { get; set; }
        /// <summary>
        /// Контактный email
        /// </summary>
        public string? ContactEmail { get; set; }
        /// <summary>
        /// Люди что могут дествовать от имени этого контрагента. Добавлять товары, менять цены и т.д.
        /// </summary>
        public List<PartnerUser> Employes { get; set; } = new List<PartnerUser>();
        public PartnerState State { get; set; }
        public PartnerType Type { get; set; }

        /// <summary>
        /// Адресс откуда забирать товар.
        /// </summary>
        [Column(TypeName = "jsonb")]
        public AddressDto Address { get; set; }

        /// <summary>
        /// Адрес регистрации юр лица.
        /// </summary>
        [Column(TypeName = "jsonb")]
        public AddressDto RegistrationAddress { get; set; }

        /// <summary>
        /// Часы работы 
        /// </summary>
        public Period? WorkingTime { get; set; } = new Period();
        /// <summary>
        /// Дни работы
        /// </summary>
        [Column(TypeName = "jsonb")]
        public List<DayOfWeek> WorkingDays { get; set; } = new();
        /// <summary>
        /// Рейтинг этого фрилансера
        /// </summary>
        public double? Rating { get; set; }

        public List<ShopDto> Shops { get; set; } = new();

        public bool IsPickupEnabled { get; set; }
        /// <summary>
        /// Идентификатор в платежной системе (в YKassa сейчас)
        /// </summary>
        public string? ExternalId { get; set; }

        //public Partner ToPartner() => Type switch
        //{
        //    PartnerType.Company => new CompanyPartner(
        //    Id, Title, Description ?? null, Created, Address?.ToAddress() ?? null, Employes is null || Employes.Count == 0 ? null : Employes.First().UserId,
        //    ImageId ?? null, Inn, ContactPhone, ContactEmail, PaymentMethod ?? null, WorkingTime?.ToPerion() ?? null, WorkingDays ?? null,
        //    Rating ?? null, State.ToState(), Shops.SelectMany(x => x.Goods.Select(x => new MerchId(x.Id))).ToList(),
        //   Shops.SelectMany(x => x.Offers.Select(x => new OfferId(x.Id))).ToList(),
        //    Shops.SelectMany(x => x.OrderItems.Select(x => new OrderId(x.OrderId))).ToList(),
        //   Ratings.Select(x => new PartnerRating(x.Id, x.Rating, x.EvaluatorId)).ToList(),
        //   new List<PartnerEvent>()),
        //    PartnerType.IndividualEntrepreneur => new IndividualEnterpreneurPartner(
        //    Id, Title, Description ?? null, Created, Address?.ToAddress() ?? null, Employes is null || Employes.Count == 0 ? null : Employes.First().UserId,
        //    ImageId ?? null, Inn, ContactPhone, ContactEmail, PaymentMethod ?? null, WorkingTime?.ToPerion() ?? null, WorkingDays ?? null,
        //    Rating ?? null, State.ToState(), Shops.SelectMany(x => x.Goods.Select(x => new MerchId(x.Id))).ToList(),
        //   Shops.SelectMany(x => x.Offers.Select(x => new OfferId(x.Id))).ToList(),
        //    Shops.SelectMany(x => x.OrderItems.Select(x => new OrderId(x.OrderId))).ToList(),
        //   Ratings.Select(x => new PartnerRating(x.Id, x.Rating, x.EvaluatorId)).ToList(),
        //   new List<PartnerEvent>()),
        //    PartnerType.SelfEmployed => new SelfEmployedPartner(
        //    Id,
        //    Title,
        //    Description ?? null,
        //    Created,
        //    Address?.ToAddress() ?? null,
        //   Employes is null || Employes.Count == 0 ? null : Employes.First().UserId,
        //    ImageId ?? null,
        //    Inn,
        //    ContactPhone,
        //    ContactEmail,
        //    PaymentMethod ?? null,
        //    WorkingTime?.ToPerion() ?? null,
        //    WorkingDays ?? null,
        //    Rating ?? null, State.ToState(),
        //    Shops.SelectMany(x => x.Goods.Select(x => new MerchId(x.Id))).ToList(),
        //   Shops.SelectMany(x => x.Offers.Select(x => new OfferId(x.Id))).ToList(),
        //    Shops.SelectMany(x => x.OrderItems.Select(x => new OrderId(x.OrderId))).ToList(),
        //   Ratings.Select(x => new PartnerRating(x.Id, x.Rating, x.EvaluatorId)).ToList(),
        //   new List<PartnerEvent>()),
        //    _ => throw new NotImplementedException()
        //};

        //public PartnerDto(Partner entity)
        //{
        //    Id = entity.Id;
        //    Title = entity.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Description = entity.Description;
        //    ImageId = entity.ImageId;
        //    Inn = entity.Inn;
        //    PaymentMethod = entity.PaymentMethod;
        //    ContactPhone = entity.ContactPhone;
        //    ContactEmail = entity.ContactEmail;
        //    Employes = new List<PartnerUser> { new PartnerUser { PartnerId = Id, UserId = entity.OwnerId } };
        //    State = entity.State.ToState();
        //    Type = entity.ToType();
        //    Address = new AddressDto(entity.Address);
        //    WorkingTime = new Period(entity.WorkingTime);
        //    WorkingDays = entity.WorkingDays;
        //}

        //public PartnerDto()
        //{

        //}
    }
}
