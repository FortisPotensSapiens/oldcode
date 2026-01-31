using Hike.Entities;

namespace Hike.Models
{
    public class ApplicationDetailsReadModel : ApplicationReadModel
    {
        public List<OfferReadModel> Offers { get; set; } = new List<OfferReadModel>();
        public UserPorfileShortInfoModel Customer { get; set; }

        public static new ApplicationDetailsReadModel From(ApplicationDto dto, string baseUri)
        {
            if (dto == null)
                return null;
            var selected = dto?.Offers.FirstOrDefault(x => x.OrderId.HasValue);
            return new ApplicationDetailsReadModel
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                SumFrom = dto.SumFrom?.Value,
                SumTo = dto.SumTo?.Value,
                CustomerId = dto.CustomerId,
                Offers = dto.Offers?.Select(x => OfferReadModel.From(x, baseUri)).ToList(),
                Created = dto.Created,
                Updated = dto.Updated,
                Number = dto.Number,
                Customer = dto?.Customer == null ? null : new UserPorfileShortInfoModel
                {
                    Id = dto.Customer.Id,
                    UserName = dto.Customer.UserName
                },
                SelectedOfferId = selected?.Id,
                SelectedOrderId = selected?.OrderId
            };
        }
    }
}
