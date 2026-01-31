using System.IO;
using System.Text;
using System.Threading.Tasks;
using Daf.SharedModule.Domain;
using Hike.Extensions;
using SixLabors.ImageSharp;

namespace Daf.FilesModule.Domain
{
    public record FileData : ValueObject<Stream>, IAsyncDisposable, IDisposable
    {
        public FileData(Stream value) : base(value)
        {
            if (value is null)
                throw new ApplicationException("Stream is null!");
        }

        public static implicit operator Stream(FileData value) => value.Value;
        public static implicit operator FileData(Stream value) => value == null ? null : new FileData(value);

        public async Task<FileHash> ComputeHash() => await Value.ComputeHash();

        public void Dispose()
        {
            Value.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return Value.DisposeAsync();
        }

        public FileSize GetSize() => Value.Length;

        public async Task<bool> IsImage()
        {
            try
            {
                Value.Position = 0;
                var imageInfo = Image.Identify(Value);
                if (imageInfo != null)
                    return true;
                return false;
            }
            catch (InvalidImageContentException exc)
            {
                return false;
            }
        }

        public async Task<string> GetUtf8Text()
        {
            await using var ms = await Value.ToMemoryStream();
            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
