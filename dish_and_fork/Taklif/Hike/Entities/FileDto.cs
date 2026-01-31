
using Daf.FilesModule.Domain;
using Daf.SharedModule.Domain;

namespace Hike.Entities
{
    public class FileDto : EntityDtoBase
    {
        [Required]
        public byte[] Hash { get; set; }
        public long Size { get; set; }
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }
        public List<UserFileDto> UserFiles { get; set; } = new List<UserFileDto>();
        [Required]
        [MaxLength(100)]
        public string Extention { get; set; }
        public List<MerchandiseImageDto> MerchandiseImages { get; set; } = new List<MerchandiseImageDto>();
        public List<OfferToApplicationImageDto> OfferToApplicationImages { get; set; } = new List<OfferToApplicationImageDto>();

        public FileDto()
        {

        }

        public FileDto(FileEntity file)
        {
            Id = file.Id;
            Created = file.Created;
            Hash = file.Hash;
            Size = file.Size;
            ContentType = file.ContentType;
            Extention = file.Extention;
            foreach (var userFile in file.UserFiles)
            {
                UserFiles.Add(new UserFileDto(userFile, file.Id));
            }
            foreach (var merchId in file.MerchIds)
            {
                MerchandiseImages.Add(new MerchandiseImageDto { MerchandiseId = merchId, FileId = file.Id });
            }
        }

        public FileEntity ToFile()
        {
            return new FileEntity(
                Id,
                Hash,
                Size,
                ContentType,
                Extention,
                 UserFiles?.Select(x => new UserFile(x.Title, x.UserId)).ToList() ?? new(),
                 MerchandiseImages?.Select(x => new MerchId(x.MerchandiseId)).ToList() ?? new(),
                 Created = Created,
                 new List<FileEvent>()
                );
        }

        public void Applay(FileEntity file)
        {
            Hash = file.Hash;
            Size = file.Size;
            ContentType = file.ContentType;
            Extention = file.Extention;
        }
    }
}
