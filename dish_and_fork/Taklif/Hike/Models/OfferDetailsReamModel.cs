using Hike.Entities;

namespace Hike.Models
{
    public class OfferDetailsReamModel : OfferReadModel
    {
        /// <summary>
        /// Коментарии к отклику на заявку
        /// </summary>
        public List<OfferCommentReadModel> Comments { get; set; } = new List<OfferCommentReadModel>();

        /// <summary>
        /// Фотки примеров похожих работ что этот кондитер раньше делал.
        /// </summary>
        public List<FileReadModel> Images { get; set; } = new List<FileReadModel>();
        public static new OfferDetailsReamModel From(OfferToApplicationDto dto, string baseUri)
        {
            if (dto == null)
                return null;
            return new OfferDetailsReamModel
            {
                Id = dto.Id,
                ApplicationId = dto.ApplicationId,
                Date = dto.Date,
                Sum = dto.Sum.Value,
                Description = dto.Description,
                PartnerId = dto.ShopId,
                CreatorId = dto.CreatorId,
                Comments = dto.Comments?.Select(OfferCommentReadModel.From).OrderBy(x => x.Created).ToList(),
                Seller = dto.Shop?.Partner == null ? null : new SellerShortInfoReadModel
                {
                    Id = dto.Shop.Id,
                    Title = dto.Shop.Partner.Title
                },
                Number = dto.Number,
                ServingGrossWeightInKilograms = dto.ServingGrossWeightInKilograms,
                Images = dto.Images.Select(x => FileReadModel.From(x.File, baseUri)).ToList(),
                SelectedOrderId = dto.OrderId
            };
        }
    }
}
