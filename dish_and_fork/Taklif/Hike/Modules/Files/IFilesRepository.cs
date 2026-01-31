using Daf.FilesModule.Domain;
using Daf.SharedModule.Domain;
using Hike.Clients;
using Hike.Ef;
using Hike.Entities;
using Hike.Modules.Shared.SecondaryAdapters;

namespace Daf.FilesModule.SecondaryAdaptersInterfaces
{
    public interface IFilesRepository : IBaseRepository<FileEntity, FileId, FileDto>
    {
    }

    public class FilesRepository : RepositoryBase<FileEntity, FileId, FileDto>, IFilesRepository
    {
        public FilesRepository(HikeDbContext db, ICancellationTokensRepository cancellationTokens) : base(db, cancellationTokens)
        {
        }

        protected override FileDto Map(FileEntity entity)
        {
            return new FileDto(entity);
        }

        protected override FileEntity Map(FileDto entity)
        {
            return entity.ToFile();
        }

        protected override void Map(FileEntity from, FileDto to)
        {
            to.Applay(from);
        }
    }
}
