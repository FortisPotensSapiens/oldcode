using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Ef;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hike.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public RolesController(HikeDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получить список ролей которые есть в системе
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin/roles")]
        [My(MyAttribute.ADMIN)]
        [ProducesResponseType(200, Type = typeof(List<IdentityRole>))]
        public async Task<List<IdentityRole>> GetAll()
        {
            return await _db.Roles.ToListAsync();
        }
    }
}
