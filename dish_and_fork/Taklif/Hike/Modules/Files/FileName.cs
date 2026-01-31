using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.FilesModule.Domain
{
    public record FileName : NotNullOrWhitespaceString
    {
        public FileName(string value) : base(value)
        {
        }
        public static implicit operator string(FileName value) => value.Value;
        public static implicit operator FileName(string value) => new FileName(value);
        public FileExtention GetExtention() => Value.GetExtention();
        public FileContentType GetContentType() => Value.GetContentType();
    }
}
