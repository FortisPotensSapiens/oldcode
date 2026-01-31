using System.Threading.Tasks;
using Daf.FilesModule.Domain;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.PrimaryAdapters
{
    public interface IFilesService
    {
        Task<FileId> Create(FileData data, FileName name, FileContentType contentType, FileSize size, UserId userId);
        Task<BigCount> Count();
        Task<List<Domain.FileEntity>> GetAll(PageNumber page, PageSize size);
        Task<List<Domain.FileEntity>> GetAll(IEnumerable<FileId> ids);
        Task<Domain.FileEntity?> Get(FileId id);
    }
}
