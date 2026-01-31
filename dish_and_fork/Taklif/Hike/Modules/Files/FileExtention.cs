using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.Domain
{
    public record FileExtention : NotNullOrWhitespaceString
    {
        public FileExtention(string value) : base(value)
        {
        }
        public static implicit operator string(FileExtention value) => value.Value;
        public static implicit operator FileExtention(string value) => new FileExtention(value);
    }
}
