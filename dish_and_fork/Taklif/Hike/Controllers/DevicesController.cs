using System.Linq.Expressions;
using System.Threading.Tasks;
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
    [Authorize]
    [ApiController]
    [Route("api/v1")]
    public partial class DevicesController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public DevicesController(HikeDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Создать устройство с указанием Puth токена для него
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("devices")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<Guid>))]
        public async Task<Guid> Create(DeviceCreateModel model)
        {
            var userId = User.Identity.Name;
            var profile = await _db.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            var old = await _db.Devices.FirstOrDefaultAsync(x => x.FcmPushToken == model.FcmPushToken && x.UserId == profile.Id);
            if (old != null)
                return old.Id;
            var device = new DeviceDto
            {
                Id = Guid.NewGuid(),
                FcmPushToken = model.FcmPushToken,
                UserId = profile.Id
            };
            _db.Devices.Add(device);
            await _db.SaveChangesAsync();
            return device.Id;
        }

        /// <summary>
        /// Получить устройства текущего пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("devices/my")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<DeviceReadModel>))]
        public async Task<PageResultModel<DeviceReadModel>> GetAll([FromQuery, Required] PaginationModel model)
        {
            var userId = User.Identity.Name;
            Expression<Func<DeviceDto, bool>> filter = x => x.UserId == userId;
            var dtos = await _db.Devices
                .Where(filter)
                .OrderByDescending(x => x.Created)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            var count = await _db.Devices
                .Where(filter)
                .CountAsync();
            return new PageResultModel<DeviceReadModel>
            {
                Items = dtos.Select(x => new DeviceReadModel { Id = x.Id, FcmPushToken = x.FcmPushToken }).ToList(),
                TotalCount = count
            };
        }

        /// <summary>
        /// Обновить данные устройства
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("devices")]
        [ProducesResponseType(200)]
        public async Task Update(DeviceUpdateModel model)
        {
            var old = await _db.Devices.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (old == null)
                new { old, model }.ThrowApplicationException("Не удалось найти устройство с таким Id!");
            old.FcmPushToken = model.FcmPushToken;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить устройство
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("devices/{id}")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<CategoryReadModel>))]
        public async Task Remove([FromRoute, Required] Guid id)
        {
            var old = await _db.Devices.FirstOrDefaultAsync(x => x.Id == id);
            if (old == null)
                return;
            _db.Devices.Remove(old);
            await _db.SaveChangesAsync();
        }
    }
}
