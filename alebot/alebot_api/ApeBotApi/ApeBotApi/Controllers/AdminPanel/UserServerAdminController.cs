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
    /// Сервера VDS пользователя
    /// </summary>
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/user-server-admin")]
    public class UserServerAdminController : ControllerBase
    {
        private readonly ILogger<UserServerAdminController> _logger;
        private readonly AbDbContext _db;

        public UserServerAdminController(ILogger<UserServerAdminController> logger, AbDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// Получить список серверов VDS пользователя
        /// </summary>
        [HttpGet("user-servers")]
        [ProducesResponseType(200, Type = typeof(List<AbUserServerDbDto>))]
        public async Task<IActionResult> GetUserServers(Guid? userId, Guid? serverId, string? login, string? address)
        {
            var query = _db.UserServers.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (serverId.HasValue)
                query = query.Where(x => x.ServerId == serverId);

            if (!string.IsNullOrWhiteSpace(login))
                query = query.Where(x => EF.Functions.ILike(x.Login, $"%{login}%"));

            if (!string.IsNullOrWhiteSpace(address))
                query = query.Where(x => EF.Functions.ILike(x.Address, $"%{address}%"));

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Получить сервер VDS пользователя
        /// </summary>
        [HttpGet("user-servers/{userServerId}")]
        [ProducesResponseType(200, Type = typeof(AbUserServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> GetUserServer(Guid userServerId)
        {
            var userServer = await _db.UserServers.FirstOrDefaultAsync(x => x.Id == userServerId);
            if (userServer == null)
                return NotFound($"Не найден сервер VDS пользователя {userServer}");

            return Ok(userServer);
        }

        /// <summary>
        /// Создать сервер VDS пользователя
        /// </summary>
        [HttpPost("user-servers")]
        [ProducesResponseType(200, Type = typeof(AbUserServerDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> CreateUserServer(CreateUserServerBinding binding)
        {
            var userServer = await _db.UserServers.FirstOrDefaultAsync(x => x.UserId == binding.UserId
                && x.ServerId == binding.ServerId
                && x.Address == binding.Address
                && x.Login == binding.Login);
            if (userServer != null)
                return ValidationProblem($"Уже есть сервер VDS {binding.ServerId} пользователя {binding.UserId} с адресом {binding.Address} и логином {binding.Login}");

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == binding.UserId);
            if (user == null)
                return NotFound($"Пользователь {binding.UserId} не найден");

            var server = await _db.Servers.FirstOrDefaultAsync(x => x.Id == binding.ServerId);
            if (server == null)
                return NotFound($"Сервер VDS {binding.ServerId} не найдена");

            userServer = new AbUserServerDbDto
            {
                UserId = binding.UserId,
                ServerId = binding.ServerId,
                Login = binding.Login,
                Password = binding.Password,
                Address = binding.Address,
            };

            _db.UserServers.Add(userServer);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUserServer Exc");
                return UnprocessableEntity("Не удалось создать сервер VDS пользователя");
            }

            return await GetUserServer(userServer.Id);
        }

        /// <summary>
        /// Удалить сервер VDS пользователя
        /// </summary>
        [HttpDelete("user-servers/{userServerId}")]
        [ProducesResponseType(200, Type = typeof(AbUserServerDbDto))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(422, Type = typeof(string))]
        public async Task<IActionResult> DeleteUserServer(Guid userServerId)
        {
            var userServer = await _db.UserServers.FirstOrDefaultAsync(x => x.Id == userServerId);
            if (userServer == null)
                return NotFound($"Не найден сервер VDS пользователя {userServerId}");

            _db.UserServers.Remove(userServer);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteUserServer Exc");
                return UnprocessableEntity("Не удалось удалить сервер VDS пользователя");
            }

            return Ok();
        }
    }
}
