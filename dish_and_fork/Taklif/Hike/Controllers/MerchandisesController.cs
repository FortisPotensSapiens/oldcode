using System.Security.Claims;
using System.Threading.Tasks;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Attributes;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Hike.Services;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    /// <summary>
    /// Товары и услуги что мы продаем
    /// </summary>
    [ApiController]
    [Route("api/v1")]
    public sealed class MerchandisesController : ControllerBase
    {
        private readonly HikeDbContext _db;
        private readonly IEmailClient _email;
        private readonly IBaseUriRepository _baseUri;

        public MerchandisesController(
            HikeDbContext db,
            IEmailClient email,
            IBaseUriRepository baseUri
            )
        {
            _db = db;
            _email = email;
            _baseUri = baseUri;
        }

        /// <summary>
        /// Запросить состав товара
        /// </summary>
        /// <returns></returns>
        [HttpPut("goods/{id}/request-composition")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchandiseReadModel>))]
        public async Task RequestComposition([FromRoute, Required] Guid id)
        {
            var merch = await _db.Merchandises.Include(x => x.CompositionRequests)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (merch == null)
                new { id }.ThrowApplicationException("Не найден товар!");
            var userId = User.Identity.Name;
            if (merch.CompositionRequests.Any(r => r.UserId == userId))
                return;
            merch.CompositionRequests.Add(new MerchandiseCompositionRequester
            {
                UserId = userId,
                MerchandiseId = id
            });
            await _db.SaveChangesAsync();
            var parnter = await _db.Partners
                .AsNoTracking()
                .Include(x => x.Employes)
                .ThenInclude(x => x.User)
                .FirstAsync(x => x.Shops.Any(s => s.Id == merch.ShopId));
            var patth = new Uri(_baseUri.Get(), $"/product/{merch.Id}");
            await _email.SendEmailAsync(
                 !string.IsNullOrWhiteSpace(parnter.ContactEmail) ? parnter.ContactEmail : parnter.Employes.FirstOrDefault()?.User?.Email,
                 $"Получен запрос на состав товара {merch.Title}. Всего запросов было  {merch.CompositionRequests.Count}. ",
                 $"<div>Получен запрос на состав товара <a href=\"{patth.AbsoluteUri}\">{merch.Title}!</a>. Всего запросов было  {merch.CompositionRequests.Count}. </div>"
                 );
        }

        /// <summary>
        /// Получить товар по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("goods/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MerchandiseReadModel))]
        public async Task<MerchandiseReadModel> Get([FromRoute, Required] Guid id)
        {
            var dto = await _db.Merchandises
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .ThenInclude(x => x.Image)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Include(x => x.CompositionRequests)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                throw new ApplicationException("Не удалось найти товар") { Data = { ["id"] = id } };
            return MerchandiseReadModel.From(dto, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", dto.Shop?.Partner?.WorkingTime);
        }

        /// <summary>
        /// Получить список товаров для главной страницы (где у нас витрина покупателя)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("goods/filter")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchandiseReadModel>))]
        public async Task<PageResultModel<MerchandiseReadModel>> Filter(FilterMerchandiseDetailsModel model)
        {
            var q = GetFiltered(model);
            var now = DateTime.UtcNow.Hour;
            var oq = q.OrderByDescending(x => x.Shop.Partner.WorkingTime.Start.Hour <= now && x.Shop.Partner.WorkingTime.End.Hour >= now);
            if (model.Orderings != null && model.Orderings.Count > 0)
            {
                foreach (var item in model.Orderings)
                {
                    oq = item.By switch
                    {
                        MerchOrderingProps.ByMerhRating => item.Asc ? oq.ThenBy(x => x.Rating) : oq.ThenByDescending(x => x.Rating),
                        MerchOrderingProps.ByOrdersCount => item.Asc ? oq.ThenBy(x => x.OrderItems.Count()) : oq.ThenByDescending(x => x.OrderItems.Count()),
                        MerchOrderingProps.ByPartnerRating => item.Asc ? oq.ThenBy(x => x.Shop.Partner.Rating ?? 0) : oq.ThenByDescending(x => x.Shop.Partner.Rating ?? 0)
                    };
                }
                q = oq;
            }
            else
            {
                q = oq.ThenByDescending(x => x.Created);
            }
            var dtos = await q
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { X = x, Y = x.Shop.Partner.WorkingTime })
                .ToListAsync();
            var totalCount = await GetFiltered(model).CountAsync();
            var result = dtos.Select(dto => MerchandiseReadModel.From(dto.X, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", dto.Y)).Where(x => x != null).ToList();
            return new PageResultModel<MerchandiseReadModel>()
            {
                Items = result,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Получить список товаров для таба с товарами продавца (где у нас страница продавца)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("goods/filter/seller-page")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchandiseReadModel>))]
        public async Task<PageResultModel<MerchandiseReadModel>> Filter(FilterMerchandiseByPartnerDetailsModel model)
        {
            var now = DateTime.UtcNow.Hour;
            var dtos = await GetFiltered(model)
                .OrderByDescending(x => x.Shop.Partner.WorkingTime.Start.Hour <= now && x.Shop.Partner.WorkingTime.End.Hour >= now)
                .ThenByDescending(x => x.Created)
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { X = x, Y = x.Shop.Partner.WorkingTime })
                .ToListAsync();
            var totalCount = await GetFiltered(model).CountAsync();
            var result = dtos.Select(dto => MerchandiseReadModel.From(dto.X, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", dto.Y)).Where(x => x != null).ToList();
            return new PageResultModel<MerchandiseReadModel>()
            {
                Items = result,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Получить список товаров для редактирования провдцом (страница мои товары на фронте продавца)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("seller/goods/filter/seller-goods-page")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchandiseReadModel>))]
        public async Task<PageResultModel<MerchandiseReadModel>> Filter(FilterMerchandiseByPartnerForPartnerDetailsModel model)
        {
            if (!await _db.Partners.AnyAsync(p => p.Employes.Any(e => e.UserId == User.Identity.Name)))
                new { model, User?.Identity?.Name }.ThrowApplicationException("Вы не можете просматривать товары этого магазина!");
            var now = DateTime.UtcNow.Hour;
            var dtos = await GetFiltered(model)
                .OrderByDescending(x => x.Shop.Partner.WorkingTime.Start.Hour <= now && x.Shop.Partner.WorkingTime.End.Hour >= now)
                .ThenByDescending(x => x.Created)
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { X = x, Y = x.Shop.Partner.WorkingTime })
                .ToListAsync();
            var totalCount = await GetFiltered(model).CountAsync();
            var result = dtos.Select(dto => MerchandiseReadModel.From(dto.X, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", dto.Y)).Where(x => x != null).ToList();
            return new PageResultModel<MerchandiseReadModel>()
            {
                Items = result,
                TotalCount = totalCount
            };
        }


        /// <summary>
        /// Получить список товаров для редактирования Админом (страница управление товарами на фронте продавца)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("admin/goods/filter")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(PageResultModel<MerchandiseReadModel>))]
        public async Task<PageResultModel<MerchandiseReadModel>> Filter(FilterMerchandiseForAdminDetailsModel model)
        {
            var now = DateTime.UtcNow.Hour;
            var dtos = await GetFiltered(model)
                .OrderByDescending(x => x.Shop.Partner.WorkingTime.Start.Hour <= now && x.Shop.Partner.WorkingTime.End.Hour >= now)
                .ThenByDescending(x => x.Created)
                .Include(x => x.Shop)
                .ThenInclude(x => x.Partner)
                .Include(x => x.Images)
                .ThenInclude(x => x.File)
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .Select(x => new { X = x, Y = x.Shop.Partner.WorkingTime })
                .ToListAsync();
            var totalCount = await GetFiltered(model)
                .CountAsync();
            var result = dtos.Select(dto => MerchandiseReadModel.From(dto.X, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", dto.Y)).Where(x => x != null).ToList();
            return new PageResultModel<MerchandiseReadModel>()
            {
                Items = result,
                TotalCount = totalCount
            };
        }


        /// <summary>
        /// Добавляет новый товар в список товаров продавца
        /// </summary>
        /// <remarks>
        /// Добавить может только пользователь с ролью "seller"
        /// Владельцем автоматически назначается пользователь что добавил товар.
        /// Для админа будет отдельный метод да и вообще пока что админим через БД напрямую. Это метод именно для зарегестрированного продавца.
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("seller/goods")]
        [Authorize]
        [My(MyAttribute.SELLER)]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(MerchandiseCreateModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var seller = await _db.Partners.AsNoTracking().FirstOrDefaultAsync(x => x.Employes.Any(y => y.UserId == userId && y.Position == UserPosition.Director));
            if (seller == null)
                throw new ApplicationException("Вы не зарегестрированны как директор магазина");
            var dto = model.ToMerchandiseDto(seller.Id);
            dto.Validate();
            _db.Merchandises.Add(dto);
            await _db.SaveChangesAsync();
            return dto.Id;
        }

        [HttpPut("seller/goods")]
        [ProducesResponseType(200, Type = typeof(int))]
        [Authorize]
        [My(MyAttribute.SELLER)]
        public async Task<int> Update(MerchandiseUpdateModel model)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var partners = await _db.Partners
                .AsNoTracking()
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            if (partners.Count == 0)
                throw new ApplicationException("Вы не зарегестрированны как директор магазина");
            var dto = await _db.Merchandises
                .Include(x => x.Images)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == model.Id && partners.Select(p => (Guid?)p).Contains(x.Shop.PartnerId));
            if (dto == null)
                throw new ApplicationException("Не удалось найти товар") { Data = { ["id"] = model.Id } };
            model.Update(dto);
            dto.Validate();
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить данные товара админом
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpPut("admin/goods")]
        [ProducesResponseType(200, Type = typeof(int))]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        public async Task<int> UpdateByAdmin(AdminMerchandiseUpdateModel model)
        {
            var dto = await _db.Merchandises
                .Include(x => x.Images)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (dto == null)
                throw new ApplicationException("Товар не найден!")
                {
                    Data = { ["args"] = model }
                };
            model.Update(dto);
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpDelete("seller/goods/{id}")]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> RemoveMerch(Guid id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var merch = await _db.Merchandises
                .Include(x => x.Ratings)
                .FirstOrDefaultAsync(x => x.Id == id && pids.Cast<Guid?>().Contains(x.ShopId));
            if (merch == null)
                throw new ApplicationException("Не удалость найти товар у этого продавца!")
                {
                    Data = { ["args"] = new { id, pids } }
                };
            _db.Merchandises.Remove(merch);
            return await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Опубликовать товар
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPut("seller/goods/{id}/publish")]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task PublishMerch(Guid id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var merch = await _db.Merchandises.FirstOrDefaultAsync(x => x.Id == id && pids.Cast<Guid?>().Contains(x.ShopId));
            if (merch == null)
                throw new ApplicationException("Не удалось найти товар") { Data = { ["id"] = id } };
            merch.State = MerchandisesState.Published;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Снять товар с публикации
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPut("seller/goods/{id}/unpublish")]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task UnpublishMerch(Guid id)
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pids = string.IsNullOrWhiteSpace(sellerId) ? new List<Guid>() : await _db.Partners
                .Where(x => x.Employes.Any(y => y.UserId == sellerId))
                .Select(x => x.Id)
                .ToListAsync();
            var merch = await _db.Merchandises.FirstOrDefaultAsync(x => x.Id == id && pids.Cast<Guid?>().Contains(x.ShopId));
            if (merch == null)
                throw new ApplicationException("Не удалось найти товар") { Data = { ["id"] = id } };
            merch.State = MerchandisesState.Created;
            await _db.SaveChangesAsync();
        }
        private IQueryable<MerchandiseDto> GetFiltered(FilterMerchandiseByPartnerForPartnerDetailsModel model)
        {
            var q = _db.Merchandises.AsNoTracking()
                   .Where(x => x.ShopId == model.PartnerId);
            if (model.ShowHidden.HasValue && model.ShowHidden.Value)
                q = q.Where(x => x.State == MerchandisesState.Created);
            if (model.ShowOnModeration.HasValue && model.ShowOnModeration.Value)
                q = q.Where(x => x.IsTagsAppovedByAdmin == false);
            if (model.ShowBlockedByAdmin.HasValue && model.ShowBlockedByAdmin.Value)
                q = q.Where(x => x.State == MerchandisesState.Blocked);
            if (model.ShowOutOfStock.HasValue && model.ShowOutOfStock.Value)
                q = q.Where(x => x.AvailableQuantity <= 0 || x.AvailableQuantity < x.ServingSize);
            if (
                model.ShowHidden.HasValue &&
                model.ShowOnModeration.HasValue &&
                model.ShowOutOfStock.HasValue &&
                model.ShowBlockedByAdmin.HasValue)
                if (!model.ShowHidden.Value &&
                    !model.ShowOnModeration.Value &&
                    !model.ShowBlockedByAdmin.Value &&
                    !model.ShowOutOfStock.Value
                    )
                    q = q.Where(x => x.State == MerchandisesState.Published)
                        .Where(x => x.IsTagsAppovedByAdmin == true)
                        .Where(x => x.AvailableQuantity > 0 && x.AvailableQuantity > x.ServingSize);
            return q;

        }

        private IQueryable<MerchandiseDto> GetFiltered(FilterMerchandiseForAdminDetailsModel model)
        {
            var q = _db.Merchandises.AsNoTracking();
            if (model.IsTagsAppovedByAdmin.HasValue)
                q = q
                   .Where(x => x.IsTagsAppovedByAdmin == model.IsTagsAppovedByAdmin);
            if (model.IsNew.HasValue)
                q = model.IsNew.Value ? q.Where(x => x.FirstTimeModerated == null) : q.Where(x => x.FirstTimeModerated != null);
            return q;
        }

        private IQueryable<MerchandiseDto> GetFiltered(FilterMerchandiseByPartnerDetailsModel model)
        {
            return _db.Merchandises.AsNoTracking()
                   .Where(x => x.ShopId == model.PartnerId)
                   .Where(x => x.State == MerchandisesState.Published)
                   .Where(x => x.AvailableQuantity > 0 && x.AvailableQuantity >= x.ServingSize)
                   .Where(x => x.Shop.Partner.State == PartnerState.Confirmed)
                   .Where(x => x.IsTagsAppovedByAdmin);
        }

        private IQueryable<MerchandiseDto> GetFiltered(FilterMerchandiseDetailsModel model)
        {
            IQueryable<MerchandiseDto> q = _db.Merchandises.AsNoTracking();
            if (model.Categories.ContainsKey(CategoryType.Kind) && model.Categories[CategoryType.Kind] is { } && model.Categories[CategoryType.Kind].Count > 0)
                q = q.Where(x => x.Categories.Any(c => model.Categories[CategoryType.Kind].Contains(c.CategoryId)));
            if (model.Categories.ContainsKey(CategoryType.Composition) && model.Categories[CategoryType.Composition] is { } && model.Categories[CategoryType.Composition].Count > 0)
                q = q.Where(x => x.Categories.Any(c => model.Categories[CategoryType.Composition].Contains(c.CategoryId)));
            if (model.Categories.ContainsKey(CategoryType.Additionally) && model.Categories[CategoryType.Additionally] is { } && model.Categories[CategoryType.Additionally].Count > 0)
                q = q.Where(x => x.Categories.Count(c => model.Categories[CategoryType.Additionally].Contains(c.CategoryId)) == model.Categories[CategoryType.Additionally].Count);
            if (!string.IsNullOrWhiteSpace(model.FindingQuery))
            {
                var fq = model.FindingQuery.GetNormalizedName();
                q = q.Where(x =>
                    x.NormalizedTitle.Contains(fq) || x.Shop.Partner.NormalizedTitle.Contains(fq));
            }
            if (model.CollectionId.HasValue)
            {
                q = q.Where(x => x.Categories.Any(c => c.Category.Collections.Any(cc => cc.CollectionId == model.CollectionId)));
            }
            return q.Where(x => x.State == MerchandisesState.Published)
                .Where(x => x.AvailableQuantity > 0 && x.AvailableQuantity >= x.ServingSize)
                .Where(x => x.Shop.Partner.State == PartnerState.Confirmed)
                .Where(x => x.IsTagsAppovedByAdmin);
        }
    }
}
