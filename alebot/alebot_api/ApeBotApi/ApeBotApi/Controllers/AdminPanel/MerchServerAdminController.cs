using AleBotApi.Bindings.Merch;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Сервера продукта
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/merch-server-admin")]
    public class MerchServerAdminController : ControllerBase
    {
        private readonly ILogger<MerchServerAdminController> _logger;
        private readonly AbDbContext _db;

        public MerchServerAdminController(ILogger<MerchServerAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список серверов продукта
        /// </summary>
        [HttpGet("merch-servers")]
        [ProducesResponseType(200, Type = typeof(List<AbMerchServerDbDto>))]
        public async Task<IActionResult> GetMerchServers(Guid? merchId, Guid? serverId)
        {
            var query = _db.MerchServers.AsNoTracking();

            if (merchId.HasValue)
                query = query.Where(x => x.MerchId == merchId);

            if (serverId.HasValue)
                query = query.Where(x => x.ServerId == serverId);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить сервер продукта
        /// </summary>
        [HttpGet("merch-servers/{merchServerId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetMerchServer(Guid merchServerId)
        {
            var merchServer = await _db.MerchServers.FirstOrDefaultAsync(x => x.Id == merchServerId);
            if (merchServer == null)
                return NotFound($"Не найден сервер продукта {merchServerId}");

            return Ok(merchServer);
        }

        /// <summary>
        /// Создать сервер продукта
        /// </summary>
        [HttpPost("merch-servers")]
        [ProducesResponseType(200, Type = typeof(AbMerchServerDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateMerchServer(CreateMerchServerBinding binding)
        {
            var merchServer = await _db.MerchServers.FirstOrDefaultAsync(x => x.MerchId == binding.MerchId
                && x.ServerId == binding.ServerId);
            if (merchServer != null)
                return ValidationProblem($"Уже есть сервер {binding.ServerId} продукта {binding.MerchId}");

            var merch = await _db.Merches.FirstOrDefaultAsync(x => x.Id == binding.MerchId);
            if (merch == null)
                return NotFound($"Продукт {binding.MerchId} не найден");

            var server = await _db.Servers.FirstOrDefaultAsync(x => x.Id == binding.ServerId);
            if (server == null)
                return NotFound($"Сервер {binding.ServerId} не найдена");

            merchServer = new AbMerchServerDbDto
            {
                MerchId = binding.MerchId,
                ServerId = binding.ServerId,
                Qty = binding.Qty,
            };

            _db.MerchServers.Add(merchServer);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateMerchServer Exc");
                return UnprocessableEntity("Не удалось создать сервер пользователя");
            }

            return await GetMerchServer(merchServer.Id);
        }

        /// <summary>
        /// Обновить сервер продукта
        /// </summary>
        [HttpPatch("merch-servers")]
        [ProducesResponseType(200, Type = typeof(AbMerchServerDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeMerchServer(ChangeMerchServerBinding binding)
        {
            var merchServer = await _db.MerchServers.FirstOrDefaultAsync(x => x.Id == binding.MerchServerId);
            if (merchServer == null)
                return NotFound($"Не найден сервер продукта {binding.MerchServerId}");

            if (merchServer.Qty == binding.Qty)
                return await GetMerchServer(merchServer.Id);

            merchServer.Qty = binding.Qty;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeMerchServer Exc");
                return UnprocessableEntity("Не удалось обновить сервер пользователя");
            }

            return await GetMerchServer(merchServer.Id);
        }

        /// <summary>
        /// Удалить сервер продукта
        /// </summary>
        [HttpDelete("merch-servers/{merchServerId}")]
        [ProducesResponseType(200, Type = typeof(AbMerchServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteMerchServer(Guid merchServerId)
        {
            var merchServer = await _db.MerchServers.FirstOrDefaultAsync(x => x.Id == merchServerId);
            if (merchServer == null)
                return NotFound($"Не найден сервер продукта {merchServerId}");

            _db.MerchServers.Remove(merchServer);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMerchServer Exc");
                return UnprocessableEntity("Не удалось удалить сервер продукта");
            }

            return Ok();
        }
    }
}
