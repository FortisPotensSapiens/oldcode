using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hike.Repositories
{
    public class ReturnUrlRepository : IReturnUrlRepository
    {
        private readonly IHttpContextAccessor _accessor;
        private const string KEY = "ReturnUrl";

        public ReturnUrlRepository(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public async Task<string> GetAsync()
        {
            await _accessor.HttpContext.Session.LoadAsync();
            return _accessor.HttpContext.Session.GetString(KEY);
        }

        public async Task SetAsync(string url)
        {
            await _accessor.HttpContext.Session.LoadAsync();
            _accessor.HttpContext.Session.SetString(KEY, url);
            await _accessor.HttpContext.Session.CommitAsync();
        }
    }
}