using System.Threading.Tasks;
using Daf.FilesModule.Domain;

namespace Daf.FilesModule.SecondaryAdaptersInterfaces
{
    public interface IFileSystemClient
    {
        Task SaveFile(FileHash hash, FileExtention extention, FileData data, FileSize size);
        Task<FileData> Get(FileHash hash, FileExtention extention);
        Task<bool> Exists(FileHash hash, FileExtention extention);
        Task<FileData> GetAdminSettingsJson();
        Task SetAdminSettingsJson(FileData data);
    }
}
