using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;
using Hike.Clients;
using Hike.Ef;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Hike.Modules.Shared.SecondaryAdapters
{

    public abstract class RepositoryBase<TEntity, TKey, TDbDto> : IBaseRepository<TEntity, TKey, TDbDto>

         where TKey : ValueObject<Guid>
         where TDbDto : EntityDtoBase, new()
         where TEntity : IEntity<TKey>
    {
        protected readonly HikeDbContext _db;
        protected readonly ICancellationTokensRepository _cancellationTokens;

        public RepositoryBase(
            HikeDbContext db,
            ICancellationTokensRepository cancellationTokens
            )
        {
            _db = db;
            _cancellationTokens = cancellationTokens;
        }

        protected abstract TDbDto Map(TEntity entity);
        protected abstract TEntity Map(TDbDto entity);
        protected abstract void Map(TEntity from, TDbDto to);
        protected virtual IQueryable<TDbDto> IncludeForUpSert(IQueryable<TDbDto> query) => query;

        public async Task Create(IEnumerable<TEntity> entities)
        {
            _db.Set<TDbDto>().AddRange(entities.Select(e => Map(e)));
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
        }

        public async Task Delete(IEnumerable<TKey> keys)
        {
            var ks = keys.Select(x => x.Value).ToList();
            await _db.Set<TDbDto>()
                .Where(x => ks.Contains(x.Id))
                .ExecuteDeleteAsync(_cancellationTokens.GetDefault());
        }

        public async Task Update(IEnumerable<TEntity> entities)
        {
            var es = entities.ToList();
            var ids = es.Select(x => x.Id.Value).ToList();
            var dtos = await IncludeForUpSert(_db.Set<TDbDto>()
                .Where(x => ids.Contains(x.Id)))
                .ToListAsync(_cancellationTokens.GetDefault());
            foreach (var dto in dtos)
            {
                var e = es.FirstOrDefault(x => x.Id.Value == dto.Id);
                if (e != null)
                    Map(e, dto);
            }
            await _db.SaveChangesAsync(_cancellationTokens.GetDefault());
        }

        public async Task<List<TEntity>> GetFiltered(Func<HikeDbContext, IQueryable<TDbDto>> filter)
        {
            var dtos = await filter(_db).AsNoTracking().ToListAsync(_cancellationTokens.GetDefault());
            return dtos.Select(x => Map(x)).ToList();
        }

        public async Task<BigCount> GetCount(Func<HikeDbContext, IQueryable<TDbDto>> filter)
        {
            var c = await filter(_db).AsNoTracking().LongCountAsync(_cancellationTokens.GetDefault());
            return new BigCount(c);
        }
    }
}
