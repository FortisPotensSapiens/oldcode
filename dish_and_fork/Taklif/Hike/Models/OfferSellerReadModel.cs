using Hike.Entities;

namespace Hike.Models
{
    public class OfferSellerReadModel : OfferReadModel
    {
        public ApplictaionShorInfoModel Applictaion { get; set; }
        public static OfferSellerReadModel From(OfferToApplicationDto dto)
        {
            if (dto == null)
                return null;
            return new OfferSellerReadModel
            {
                Id = dto.Id,
                ApplicationId = dto.ApplicationId,
                Applictaion = ApplictaionShorInfoModel.From(dto.Application),
                Date = dto.Date,
                Sum = dto.Sum.Value,
                Description = dto.Description,
                PartnerId = dto.ShopId,
                CreatorId = dto.CreatorId,
                Seller = dto?.Shop?.Partner == null ? null : new SellerShortInfoReadModel
                {
                    Id = dto.Id,
                    Title = dto.Shop?.Partner?.Title
                },
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms,
                SelectedOrderId = dto.OrderId
            };
        }
    }
}
