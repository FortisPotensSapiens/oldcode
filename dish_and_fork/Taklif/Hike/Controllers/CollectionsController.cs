using Hike.Attributes;
using Hike.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hike.Ef;
using Hike.Entities;
using Hike.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [ApiController]
    [Route("api/v1/collections")]
    public sealed class CollectionsController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public CollectionsController(HikeDbContext db)
        {
            _db = db;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CollectionReadModel))]
        public async Task<CollectionReadModel> Get(Guid id)
        {
            var dto = await _db.Collections.FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                new { id }.ThrowApplicationException("Коллекция не найдена!");
            return CollectionReadModel.From(dto);
        }

        /// <summary>
        /// Создать коллецкцию
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpPost("admin")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(CollectionCreateModel model)
        {
            if (await _db.Collections.CountAsync() >= 4)
                new { model }.ThrowApplicationException("Уже создано 4 коллекции. Удалите старую чтобы создать новую!");
            if (await _db.Collections.AnyAsync(x => x.NormalizedTitle == model.Title.GetNormalizedName()))
                new { model }.ThrowApplicationException("Уже есть коллекция с таким названием!");
            var c = model.ToCollection();
            _db.Collections.Add(c);
            _db.CollectionCategories.AddRange(c.Categories);
            await _db.SaveChangesAsync();
            return c.Id;
        }

        /// <summary>
        /// Получить список коллекция для админа
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpGet("admin")]
        [ProducesResponseType(200, Type = typeof(List<CollectionReadModel>))]
        public async Task<List<CollectionReadModel>> GetAll()
        {
            var dtos = await _db
                .Collections
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .ToListAsync();
            return dtos.Select(x => CollectionReadModel.From(x)).ToList();
        }

        /// <summary>
        /// Получить список коллекций для покупателя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CollectionReadModel>))]
        public async Task<List<CollectionReadModel>> GetAllForUser()
        {
            var dtos = await _db
                .Collections
                .Include(x => x.Categories)
                .ThenInclude(x => x.Category)
                .ToListAsync();
            return dtos.Select(x => CollectionReadModel.From(x)).ToList();
        }

        /// <summary>
        /// Обновить коллекцию
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpPut("admin")]
        public async Task Update(CollectionUpdateModel model)
        {
            if (await _db.Collections.AnyAsync(x => x.NormalizedTitle == model.Title.GetNormalizedName() && x.Id != model.Id))
                new { model }.ThrowApplicationException("Уже есть коллекция с таким названием!");
            var dto = await _db.Collections
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            if (dto == null)
                new { model }.ThrowApplicationException("Не удалось найти такую коллекцию");
            model.Applay(dto);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить коллекцию
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.ADMIN)]
        [HttpDelete("admin/{id}")]
        public async Task Delete(Guid id)
        {
            var dto = await _db.Collections.FirstOrDefaultAsync(x => x.Id == id);
            if (dto == null)
                return;
            _db.Collections.Remove(dto);
            await _db.SaveChangesAsync();
        }
    }
}
