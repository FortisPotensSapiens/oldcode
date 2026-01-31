using Daf.SharedModule.Domain.BaseVo;
using Hike.Ef;
using System.Threading.Tasks;

namespace Hike.Modules.Shared.SecondaryAdapters
{
    public interface IBaseRepository<TEntity, TKey, TFilter>
    {
        Task<List<TEntity>> GetFiltered(Func<HikeDbContext, IQueryable<TFilter>> filter);
        Task<BigCount> GetCount(Func<HikeDbContext, IQueryable<TFilter>> filter);
        Task Create(IEnumerable<TEntity> entities);
        Task Update(IEnumerable<TEntity> entities);
        Task Delete(IEnumerable<TKey> keys);
    }
}
