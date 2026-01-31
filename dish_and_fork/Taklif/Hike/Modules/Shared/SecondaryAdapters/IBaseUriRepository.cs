using Microsoft.AspNetCore.Http;

namespace Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories
{
    public interface IBaseUriRepository
    {
        Uri Get();
    }

    public class BaseUriRepository : IBaseUriRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseUriRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Uri Get()
        {
            var r = _httpContextAccessor.HttpContext?.Request;
            var s = $"{r?.Scheme}://{r?.Host}";
            return new Uri(s, UriKind.Absolute);
        }
    }
}
