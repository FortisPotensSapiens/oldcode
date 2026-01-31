using System.Threading.Tasks;

namespace Hike.Repositories
{
    public interface IReturnUrlRepository
    {
        Task<string> GetAsync();
        Task SetAsync(string url);
    }
}
