namespace Hike.Entities
{
    public class ApplicationDto : DescribedDtoBase
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public MoneyDto? SumFrom { get; set; }
        public MoneyDto? SumTo { get; set; }
        public HikeUser Customer { get; set; }
        [Required]
        public string CustomerId { get; set; }

        public List<OfferToApplicationDto> Offers { get; set; } = new List<OfferToApplicationDto>();
        public long Number { get; set; }

        //public void Applay(ApplicationEntity entity)
        //{
        //    Title = entity.Title;
        //    Description = entity.Description;
        //    FromDate = entity.FromDate;
        //    ToDate = entity.ToDate;
        //    SumFrom = entity.SumFrom == null ? null : new MoneyDto(entity.SumFrom);
        //    SumTo = entity.SumTo == null ? null : new MoneyDto(entity.SumTo);
        //    CustomerId = entity.UserId;
        //    ApplayEvents(this, entity.Events);
        //}

        //private void ApplayEvents(ApplicationDto dto, IEnumerable<ApplicationsEvent> events)
        //{
        //    foreach (var e in events)
        //    {
        //        if (e is OfferAdded added)
        //        {
        //            dto.Offers.Add(new OfferToApplicationDto(added.offer));
        //        }
        //    }
        //}

        //public ApplicationDto()
        //{

        //}

        //public ApplicationDto(ApplicationEntity entity)
        //{
        //    Id = entity.Id;
        //    Title = entity.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Created = entity.Created;
        //    FromDate = entity.FromDate;
        //    ToDate = entity.ToDate;
        //    SumFrom = entity.SumFrom == null ? null : new MoneyDto(entity.SumFrom);
        //    SumTo = entity.SumTo == null ? null : new MoneyDto(entity.SumTo);
        //    CustomerId = entity.UserId;
        //    Number = entity.Number;
        //    ApplayEvents(this, entity.Events);
        //}

        //public ApplicationEntity ToApplication() =>
        //    new ApplicationEntity(
        //        Id,
        //        CustomerId,
        //        Title,
        //        Description,
        //        FromDate,
        //        ToDate,
        //        SumFrom?.ToMoney(),
        //        SumTo?.ToMoney(),
        //        Created,
        //        Number,
        //        Offers?.Select(f => f.ToOffer()).ToList() ?? new(),
        //        new ApplicationsEvent[0]
        //        );

      //  public void Validate()
      //  {
      //      if (ToDate.HasValue && ToDate.Value.Date < (Created == default ? DateTime.Now : Created).Date)
      //          throw new ApplicationException("Дата до раньще даты публикации заказа!")
      //          {
      //              Data = { ["args"] = this }
      //          };
      //      if (ToDate.HasValue && FromDate.HasValue &&
      //   ToDate.Value.Date < FromDate.Value.Date)
      //          throw new ApplicationException("Дата с больше даты по")
      //          {
      //              Data = { ["args"] = this }
      //          };
      //      if (SumTo is { } && SumFrom is { } &&
      //SumTo.Value - SumFrom.Value < 0)
      //          throw new ApplicationException("Сумма с больше суммы по")
      //          {
      //              Data = { ["args"] = this }
      //          };
      //  }
    }
}
