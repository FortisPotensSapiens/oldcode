using System.ComponentModel.DataAnnotations;
using AleBotApi.Bindings.Server;
using AleBotApi.DbContexts;
using AleBotApi.DbDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.Controllers.AdminPanel
{
    /// <summary>
    /// Сервера VDS
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/server-admin")]
    public class ServerAdminController : ControllerBase
    {
        private readonly ILogger<ServerAdminController> _logger;
        private readonly AbDbContext _db;

        public ServerAdminController(ILogger<ServerAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список серверов VDS
        /// </summary>
        [HttpGet("servers")]
        [ProducesResponseType(200, Type = typeof(List<AbServerDbDto>))]
        public async Task<IActionResult> GetServers()
        {
            return Ok(await _db.Servers
                .AsNoTracking()
                .ToListAsync());
        }

        /// <summary>
        /// Получить сервер VDS
        /// </summary>
        [HttpGet("servers/{serverId}")]
        [ProducesResponseType(200, Type = typeof(AbServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetServer(Guid serverId)
        {
            var server = await _db.Servers.FirstOrDefaultAsync(x => x.Id == serverId);
            if (server == null)
                return NotFound($"Не найден сервер VDS {serverId}");

            return Ok(server);
        }

        /// <summary>
        /// Создать сервер VDS
        /// </summary>
        [HttpPost("servers")]
        [ProducesResponseType(200, Type = typeof(AbServerDbDto))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateServer([FromBody][Required] CreateServerBinding binding)
        {
            var server = new AbServerDbDto
            {
                Name = binding.Name.Trim()
            };

            _db.Servers.Add(server);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateServer Exc");
                return UnprocessableEntity("Не удалось создать сервер VDS");
            }

            return await GetServer(server.Id);
        }

        /// <summary>
        /// Изменить сервер VDS
        /// </summary>
        [HttpPatch("servers")]
        [ProducesResponseType(200, Type = typeof(AbServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> ChangeServer([FromBody][Required] ChangeServerBinding binding)
        {
            var server = await _db.Servers.FirstOrDefaultAsync(x => x.Id == binding.ServerId);
            if (server == null)
                return NotFound($"Не найден сервер VDS {binding.ServerId}");

            if (server.Name == binding.Name)
                return Ok(server);

            server.Name = binding.Name;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeServer Exc");
                return UnprocessableEntity("Не удалось изменить сервер VDS");
            }

            return await GetServer(server.Id);
        }

        /// <summary>
        /// Удалить сервер VDS
        /// </summary>
        [HttpDelete("servers/{serverId}")]
        [ProducesResponseType(200, Type = typeof(AbServerDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteServer(Guid serverId)
        {
            var server = await _db.Servers.FirstOrDefaultAsync(x => x.Id == serverId);
            if (server == null)
                return NotFound($"Не найден сервер VDS {serverId}");

            var userServersCount = await _db.UserServers.Where(x => x.ServerId == serverId).CountAsync();
            if (userServersCount > 0)
                return ValidationProblem($"Не удалось удалить сервер VDS, так как у него есть сервер VDS пользователя");

            _db.Servers.Remove(server);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteServer Exc");
                return UnprocessableEntity("Не удалось удалить сервер VDS");
            }

            return Ok();
        }
    }
}
