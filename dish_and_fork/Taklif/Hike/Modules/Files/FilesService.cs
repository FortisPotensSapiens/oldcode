using System.Threading.Tasks;
using Daf.FilesModule.Domain;
using Daf.FilesModule.SecondaryAdaptersInterfaces;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Daf.FilesModule.PrimaryAdapters
{
    public class FilesService : IFilesService
    {
        private readonly IFilesRepository _files;
        private readonly IFileSystemClient _fileSystem;
        private readonly IGuidsRepository _guids;
        private readonly IDateTimesRepository _dateTimes;

        public FilesService(
            IFilesRepository files,
            IFileSystemClient fileSystem,
            IGuidsRepository guids,
            IDateTimesRepository dateTimes
            )
        {
            _files = files;
            _fileSystem = fileSystem;
            _guids = guids;
            _dateTimes = dateTimes;
        }

        public Task<BigCount> Count()
        {
            return _files.GetCount(null);
        }

        public async Task<FileId> Create(FileData data, FileName name, FileContentType contentType, FileSize size, UserId userId)
        {
            var hash = await data.ComputeHash();
            var olds = await _files.GetFiltered(x => x.Files.AsNoTracking()
            .Where(f => f.Hash == hash.Value));
            var old = olds.FirstOrDefault();
            if (old is not null)
                return old.Id;
            var extention = name.GetExtention();
            if (!await _fileSystem.Exists(hash, extention))
                await _fileSystem.SaveFile(hash, extention, data, size);
            var id = new FileId(_guids.GetNew());
            await _files.Create(new[] { new Domain.FileEntity(id, hash, size, contentType, extention, new[] { new UserFile(name, userId) }, new MerchId[0] { }, _dateTimes.Now(), new FileEvent[0].AsReadOnly()) });
            return id;
        }

        public async Task<Domain.FileEntity?> Get(FileId id)
        {
            var files = await _files.GetFiltered(x => x.Files.AsNoTracking()
            .Where(f => f.Id == id.Value));
            return files.FirstOrDefault();
        }

        public Task<List<FileEntity>> GetAll(PageNumber page, PageSize size)
        {
            return _files.GetFiltered(x => x.Files.AsNoTracking()
            .OrderBy(f => f.Created)
            .Skip((page.Value - 1) * size.Value)
            .Take(size.Value));
        }

        public Task<List<FileEntity>> GetAll(IEnumerable<FileId> ids)
        {
            var igs = ids.Select(i => (Guid)i).ToList();
            return _files.GetFiltered(x => x.Files.AsNoTracking()
            .Where(f => igs.Contains(f.Id)));
        }
    }
}
