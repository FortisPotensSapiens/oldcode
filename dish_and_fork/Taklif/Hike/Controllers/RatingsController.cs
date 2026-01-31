using System.Data;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public partial class RatingsController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public RatingsController(HikeDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получить оценку которую покупатель ставил этому товару
        /// </summary>
        /// <param name="merchId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("my-assigned-merch-rating/{merchId}")]
        public async Task<MerchRatingReadModel> GetMyMerchRating(Guid merchId)
        {
            var dto = await _db.Ratings
                .FirstOrDefaultAsync(x =>
                x.MerchandiseId == merchId &&
                x.RatingType == RatingType.Merch &&
                x.EvaluatorId == User.Identity.Name);
            return MerchRatingReadModel.From(dto);
        }

        /// <summary>
        /// Получить отзыв который я ставил этому товару
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("my-assigned-partner-rating/{partnerId}")]
        public async Task<PartnerRatingReadModel> GetMyPartnerRating(Guid partnerId)
        {
            var dto = await _db.Ratings
                   .FirstOrDefaultAsync(x =>
                x.ShopId == partnerId &&
                x.RatingType == RatingType.Partner &&
                x.EvaluatorId == User.Identity.Name);
            return PartnerRatingReadModel.From(dto);
        }

        /// <summary>
        /// Проверяет может ли текущий пользователь поставить оценку этому товару 
        /// </summary>
        /// <param name="merchId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("can-i-set-rating-to-merch/{merchId}")]
        public async Task<bool> CanSetRatinToMerch(Guid merchId)
        {
            var userId = User.Identity.Name;
            var profile = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            return await _db.Orders
                .AnyAsync(x => x.BuyerId == profile.Id && x.Items.Any(i => i.ItemId == merchId) && x.State != OrderState.Created);
        }

        /// <summary>
        /// Проверяет может ли пользователь поставить оценку данному продавцу
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("can-i-set-rating-to-partner/{partnerId}")]
        public async Task<bool> CanSetRatinToPartner(Guid partnerId)
        {
            var userId = User.Identity.Name;
            var profile = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            return await _db.Orders
                .AnyAsync(x =>
                x.BuyerInfo.Id == profile.Id &&
                x.State != OrderState.Created &&
                x.SellerInfo.Id == partnerId &&
                x.Type == OrderType.Individual
                );
        }

        /// <summary>
        /// ДОбавить оценку товару
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("merch-ratings")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateForMerch(MerchRatingCreateModel model)
        {
            using var tran = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            if (!await _db.Orders.AnyAsync(x => x.State != OrderState.Created && x.Items.Any(y => y.ItemId == model.MerchId)))
                new { model }.ThrowApplicationException("Пользователь ни разу не покупал этот товар!");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (await _db.Ratings.AnyAsync(x => x.EvaluatorId == userProfile.Id && x.RatingType == RatingType.Merch && x.MerchandiseId == model.MerchId))
                new { model }.ThrowApplicationException("Пользователь уже ставил оценку этому товару!");
            var rating = model.ToMerchRating(userProfile.Id);
            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
            var merch = await _db.Merchandises
                .FirstOrDefaultAsync(x => x.Id == model.MerchId);
            var agvRating = await _db.GetMerchRating(merch.Id);
            merch.Rating = agvRating;
            await _db.SaveChangesAsync();
            await tran.CommitAsync();
            return rating.Id;
        }

        /// <summary>
        /// Прочитать данные оценки товара
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("merch-ratings/{id}")]
        [ProducesResponseType(200, Type = typeof(MerchRatingReadModel))]
        public async Task<MerchRatingReadModel> GetMerchRating(Guid id)
        {
            var dto = await _db.Ratings.FirstOrDefaultAsync(x => x.Id == id && x.RatingType == RatingType.Merch);
            return MerchRatingReadModel.From(dto);
        }

        /// <summary>
        /// Получить список оценок товара
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("merch-ratings/filter")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchRatingReadModel>))]
        public async Task<PageResultModel<MerchRatingReadModel>> FilterMerchRatings(MerchRatingFilterModel model)
        {
            var filter = GetMerchRatingsFilter(model);
            var dtos = await _db.Ratings
                .Where(filter)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await _db.Ratings
                .Where(filter)
                .CountAsync();
            return new PageResultModel<MerchRatingReadModel>
            {
                TotalCount = count,
                Items = dtos.Select(x => MerchRatingReadModel.From(x)).ToList()
            };
        }

        /// <summary>
        /// Добавить оценку фрилансеру 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("partner-ratings")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> CreateForPartner(PartnerRatingCreateModel model)
        {
            using var tran = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userProfile = await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (!await _db.Orders.AnyAsync(x => x.State != OrderState.Created && x.Type == OrderType.Individual && x.BuyerId == userProfile.Id && x.SellerInfo.Id == model.PartnerId))
                new { model }.ThrowApplicationException("Пользователь ни разу не делал индивидуальный заказ у этого продавца!");
            if (await _db.Ratings.AnyAsync(x => x.EvaluatorId == userProfile.Id && x.RatingType == RatingType.Partner && x.ShopId == model.PartnerId))
                new { model }.ThrowApplicationException("Пользователь уже ставил оценку этому продавцу (фрилансеру)!");
            var rating = model.ToPartnerRating(userProfile.Id);
            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
            var partner = await _db.Shops.Include(x => x.Partner).FirstOrDefaultAsync(x => x.Id == model.PartnerId);
            var agvRating = await _db.GetPartnerRating(partner.Id);
            partner.Partner.Rating = agvRating;
            await _db.SaveChangesAsync();
            await tran.CommitAsync();
            return rating.Id;
        }

        /// <summary>
        /// Прочитать данные оценки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("partner-ratings/{id}")]
        [ProducesResponseType(200, Type = typeof(PartnerRatingReadModel))]
        public async Task<PartnerRatingReadModel> GetPartnerRating(Guid id)
        {
            var dto = await _db.Ratings.FirstOrDefaultAsync(x => x.Id == id && x.RatingType == RatingType.Partner);
            return PartnerRatingReadModel.From(dto);
        }

        /// <summary>
        /// Получить список оценок фрилансеров
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("partner-ratings/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<PartnerRatingReadModel>))]
        public async Task<PageResultModel<PartnerRatingReadModel>> FilterPartnerRatings(PartnerRatingFilterModel model)
        {
            var filter = GetPartnerRatingFilter(model);
            var dtos = await _db.Ratings
                .Where(filter)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await _db.Ratings
                .Where(filter)
                .CountAsync();
            return new PageResultModel<PartnerRatingReadModel>
            {
                TotalCount = count,
                Items = dtos.Select(x => PartnerRatingReadModel.From(x)).ToList()
            };
        }

        /// <summary>
        /// Обновить поставленную оценку товару или продавцу (фрилансеру)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ratings")]
        [ProducesResponseType(200)]
        public async Task Update(RatingUpdateModel model)
        {
            using var tran = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = await _db.Ratings.FirstOrDefaultAsync(x => x.Id == model.Id && x.Evaluator.Id == userId);
            model.Applay(dto);
            await _db.SaveChangesAsync();
            if (dto.RatingType == RatingType.Partner)
            {
                var partner = await _db.Shops.Include(x => x.Partner).FirstOrDefaultAsync(x => x.Id == dto.ShopId);
                var agvRating = await _db.GetPartnerRating(partner.Id);
                partner.Partner.Rating = agvRating;
            }
            else
            {
                var merch = await _db.Merchandises
              .FirstOrDefaultAsync(x => x.Id == dto.MerchandiseId);
                var agvRating = await _db.GetMerchRating(merch.Id);
                merch.Rating = agvRating;
            }
            await _db.SaveChangesAsync();
            tran.Commit();
        }

        private static Expression<Func<RatingDto, bool>> GetPartnerRatingFilter(PartnerRatingFilterModel model)
        {
            if (!model.PartnerId.HasValue)
                return x => x.RatingType == RatingType.Partner;
            return x => x.ShopId == model.PartnerId && x.RatingType == RatingType.Partner;
        }

        private static Expression<Func<RatingDto, bool>> GetMerchRatingsFilter(MerchRatingFilterModel model)
        {
            if (!model.MerchId.HasValue)
                return x => x.RatingType == RatingType.Merch;
            return x => x.MerchandiseId == model.MerchId && x.RatingType == RatingType.Merch;
        }
    }
}
