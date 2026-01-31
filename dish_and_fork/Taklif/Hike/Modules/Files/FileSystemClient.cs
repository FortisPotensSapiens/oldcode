using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using Daf.FilesModule.Domain;
using Hike.Clients;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;

namespace Daf.FilesModule.SecondaryAdaptersInterfaces
{
    public class FileSystemClient : IFileSystemClient
    {
        private const string DIR_NAME = "files";

        private readonly IWebHostEnvironment _environment;
        private readonly ICancellationTokensRepository _cancellationTokens;

        public FileSystemClient(IWebHostEnvironment environment, ICancellationTokensRepository cancellationTokens)
        {
            _environment = environment;
            _cancellationTokens = cancellationTokens;
        }

        public async Task<bool> Exists(FileHash hash, FileExtention extention)
        {
            var dir = Path.Combine(_environment.WebRootPath, DIR_NAME);
            var path = Path.Combine(dir, FileEntity.GetDefaultName(hash, extention));
            return File.Exists(path);
        }

        public async Task<FileData> Get(FileHash hash, FileExtention extention)
        {
            var dir = Path.Combine(_environment.WebRootPath, DIR_NAME);
            var path = Path.Combine(dir, FileEntity.GetDefaultName(hash, extention));
            return new FileData(File.OpenRead(path));
        }

        public async Task<FileData> GetAdminSettingsJson()
        {
            var dir = Path.Combine(_environment.WebRootPath, "doc");
            var path = Path.Combine(dir, "golbalsettings.json");
            if (!File.Exists(path))
                return new FileData(new MemoryStream(new byte[0]));
            return new FileData(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 5_000_000));
        }

        public async Task SaveFile(FileHash hash, FileExtention extention, FileData data, FileSize size)
        {
            var dir = Path.Combine(_environment.WebRootPath, DIR_NAME);
            var path = Path.Combine(dir, FileEntity.GetDefaultName(hash, extention));
            if (File.Exists(path))
                return;
            if (await data.IsImage())
                await ClearMetadata(data.Value, path);
            else
            {
                using var file = File.Create(path, 5_000_000);
                data.Value.Position = 0;
                await data.Value.CopyToAsync(file, _cancellationTokens.GetDefault());
                await file.FlushAsync(_cancellationTokens.GetDefault());
            }
        }

        public async Task SetAdminSettingsJson(FileData data)
        {
            var dir = Path.Combine(_environment.WebRootPath, "doc");
            var path = Path.Combine(dir, "golbalsettings.json");
            using var file = File.Create(path, 5_000_000);
            data.Value.Position = 0;
            await data.Value.CopyToAsync(file, _cancellationTokens.GetDefault());
            await file.FlushAsync(_cancellationTokens.GetDefault());
        }

        private async Task ClearMetadata(Stream input, string path)
        {
            using var image = await Image.LoadAsync(input, _cancellationTokens.GetDefault());
            image.Metadata.ExifProfile = null;
            image.Metadata.IptcProfile = null;
            image.Metadata.XmpProfile = null;
            await image.SaveAsync(path, _cancellationTokens.GetDefault());
        }
    }
}
