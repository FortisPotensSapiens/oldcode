
namespace Hike.Entities
{
    public class SellerInfoDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? NormalizedTitle { get; set; }
        public string? Description { get; set; }
        public Guid? ImageId { get; set; }
        public string? Inn { get; set; }
        /// <summary>
        /// Идентификатор в платежной системе (в YKassa сейчас)
        /// </summary>
        public string? ExternalId { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public PartnerState? State { get; set; }
        public PartnerType? Type { get; set; }
        public AddressDto? Address { get; set; } = new();
        public Period? WorkingTime { get; set; } = new Period();
        public List<DayOfWeek> WorkingDays { get; set; } = new();
        public List<string> Employees { get; set; } = new();

        //public SellerInfoDto()
        //{

        //}

        //public SellerInfoDto(SellerInfo dto)
        //{
        //    Id = dto.Id;
        //    Title = dto.Title;
        //    NormalizedTitle = Title?.GetNormalizedName();
        //    Description = dto.Description;
        //    ImageId = dto.ImageId;
        //    Inn = dto.Inn;
        //    PaymentMethod = dto.PaymentMethod;
        //    ContactPhone = dto.ContactPhone;
        //    ContactEmail = dto.ContactEmail;
        //    State = PartnerState.Confirmed;
        //    Type = dto.Type;
        //    Address = new AddressDto(dto.Address);
        //    WorkingTime = new Period(dto.WorkingTime);
        //    WorkingDays = dto.WorkingDays;
        //    Employees = new[] { dto.OwnerId.Value }.ToList();
        //}


        public static SellerInfoDto From(ShopDto shop)
        {
            if (shop?.Partner == null)
                return null;
            var dto = shop.Partner;
            return new SellerInfoDto
            {
                Id = shop.Id,
                Title = dto.Title,
                NormalizedTitle = dto.NormalizedTitle,
                Description = dto.Description,
                ImageId = dto.ImageId,
                Inn = dto.Inn,
                ExternalId = dto.ExternalId,
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                State = dto.State,
                Type = dto.Type,
                Address = dto.Address,
                WorkingTime = dto.WorkingTime,
                WorkingDays = dto.WorkingDays,
                Employees = dto.Employes.Select(x => x.UserId).ToList(),

            };
        }
    }
}
