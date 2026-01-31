using System.ComponentModel.DataAnnotations;
using AleBotApi.DbContexts;
using AleBotApi.Models;
using ApeBotApi.DbDtos;
using ApeBotApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AleBotApi.Controllers.AdminPanel
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/sso-admin")]
    public class SsoAdminController : ControllerBase
    {
        private readonly AbDbContext _db;

        public SsoAdminController(AbDbContext db)
        {
            _db = db;
        }

        [HttpGet("users")]
        [ProducesResponseType(200, Type = typeof(PageResultModel<UserReadModel>))]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationModel model)
        {
            var count = await _db.Users.CountAsync();
            var files = await _db.Users
                .OrderByDescending(x => x.Created)
                .Skip((int)model.Skip())
                .Take((int)model.PageSize)
                .ToListAsync();
            return Ok(new PageResultModel<UserReadModel>
            {
                TotalCount = count,
                Items = files.Select(x => UserReadModel.From(x)).ToList()
            });
        }

        [HttpPut("users/add-admin-role/{userId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddAdminRole([FromRoute, Required] Guid userId)
        {
            var adminRole = await _db.Roles.FirstOrDefaultAsync(x => x.NormalizedName == "admin".GetNormalizedName());
            _db.UserRoles.Add(new UserRoleDbDto { RoleId = adminRole.Id, UserId = userId });
            await _db.SaveChangesAsync();
            return Ok();
        }

    }
}
