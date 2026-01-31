using Hike.Entities;
using Hike.Models.Base;

namespace Hike.Models
{
    public class FileReadModel : BaseTitledModel
    {
        [Obsolete]
        public FileReadModel() : base(default)
        {

        }

        public FileReadModel(Guid id, string title, string path, byte[] hash, long size, string contentType, DateTime created, DateTime? updated, string concurrencyToken) : base(title)
        {
            Id = id;
            Hash = hash;
            Size = size;
            ContentType = contentType;
            Created = created;
            Updated = updated;
            ConcurrencyToken = concurrencyToken;
            Path = path;
        }

        public Guid Id { get; set; }
        public byte[] Hash { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string ConcurrencyToken { get; set; }
        public string Path { get; set; }

        public static FileReadModel From(FileDto dto, string baseUrl)
        {
            if (dto == null)
                return null;
            return new FileReadModel(dto.Id, Convert.ToBase64String(dto.Hash) + dto.Extention, $"{(baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/")}files/{GetName(dto)}", dto.Hash, dto.Size, dto.ContentType, dto.Created, dto.Updated, dto.ConcurrencyToken);
        }

        public static string GetName(FileDto dto)
        {
            var fileName = BitConverter.ToString(dto.Hash).Replace("-", "");
            var extension = System.IO.Path.GetExtension(Convert.ToBase64String(dto.Hash) + dto.Extention);
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".file";
            return fileName + extension;
        }

    }
}
