using Daf.FilesModule.Domain;
using Hike.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hike.Entities
{
    [Index(nameof(NormalizedTitle), IsUnique = true)]
    public class UserFileDto : TitledDtoBase
    {
        public HikeUser User { get; set; }
        public string UserId { get; set; }
        public FileDto File { get; set; }
        public Guid FileId { get; set; }

        public UserFileDto(UserFile file, Guid fileId)
        {
            Title = file.File;
            UserId = file.UserId;
            FileId = fileId;
            NormalizedTitle = Title?.GetNormalizedName();
        }

        public UserFileDto()
        {

        }
    }
}
