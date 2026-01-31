using System.Data;
using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Hike.Models;
using Hike.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public sealed class CategoriesController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public CategoriesController(HikeDbContext db)
        {
            _db = db;
        }

        [HttpPost("admin/categories")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(CategoryCreateModel model)
        {
            var dto = model.ToCategory();
            var old = await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.NormalizedTitle == dto.NormalizedTitle);
            if (old != null)
                throw new ApplicationException("Такая категория уже существует")
                {
                    Data = { ["args"] = new { model, old } }
                };
            _db.Categories.Add(dto);
            await _db.SaveChangesAsync();
            return dto.Id;
        }

        [HttpGet("categories/{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryReadModel))]
        public async Task<CategoryReadModel> Get(Guid id)
        {
            var dto = await _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                throw new ApplicationException("Категория с таким id не найдена!")
                {
                    Data = { ["args"] = new { id } }
                };
            return CategoryReadModel.From(dto);
        }

        [HttpPost("categories/filter")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<CategoryReadModel>))]
        public async Task<PageResultModel<CategoryReadModel>> GetAll(CategoryFilterModel model)
        {
            var dtos = await GetFiltered(model)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await GetFiltered(model).CountAsync();
            return new PageResultModel<CategoryReadModel>
            {
                TotalCount = count,
                Items = dtos.Select(CategoryReadModel.From).ToList()
            };
        }

        private IQueryable<CategoryDto> GetFiltered(CategoryFilterModel filter)
        {
            var nt = filter.Title?.GetNormalizedName();
            var q = _db.Categories.OrderBy(x => x.Title).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(nt))
                q = q.Where(x => x.NormalizedTitle.Contains(nt));
            if (filter.HideEmpty)
                q = q.Where(x => x.MerchandiseCategories.Any(mc => mc.Merchandise.State == MerchandisesState.Published
                && mc.Merchandise.AvailableQuantity >= mc.Merchandise.ServingSize));
            return q;
        }

        [HttpPut("admin/categories")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> Update(CategoryUpdateModel model)
        {
            var dto = await _db.Categories.FindAsync(model.Id);
            if (dto == null)
                throw new ApplicationException("Категория с таким id не найдена!")
                {
                    Data = { ["args"] = new { model } }
                };
            model.Update(dto);
            return await _db.SaveChangesAsync();
        }

        [HttpDelete("admin/categories/{id}")]
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(int))]
        public async Task<int> Delete(Guid id)
        {
            var dto = await _db.Categories.FindAsync(id);
            if (dto == null)
                throw new ApplicationException("Категория с таким id не найдена!")
                {
                    Data = { ["args"] = new { id } }
                };
            _db.Categories.Remove(dto);
            return await _db.SaveChangesAsync();
        }
    }
}
