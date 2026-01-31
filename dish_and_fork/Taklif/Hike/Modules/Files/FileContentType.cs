using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.Domain
{
    public record FileContentType : NotNullOrWhitespaceString
    {
        public FileContentType(string value) : base(value)
        {
        }
        public static implicit operator string(FileContentType value) => value.Value;
        public static implicit operator FileContentType(string value) => new FileContentType(value);
    }
}
