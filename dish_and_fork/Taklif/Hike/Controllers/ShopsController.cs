using System.Threading.Tasks;
using Hike.Attributes;
using Hike.Ef;
using Hike.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hike.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly HikeDbContext _db;

        public ShopsController(HikeDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Создать магазин
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [My(MyAttribute.SELLER)]
        [HttpPost("seller/shops")]
        [ProducesResponseType(200, Type = typeof(Guid))]
        public async Task<Guid> Create(ShopCreateModel model)
        {
            var dto = model.ToShopDto();
            _db.Shops.Add(dto);
            await _db.SaveChangesAsync();
            return dto.Id;
        }
    }
}
