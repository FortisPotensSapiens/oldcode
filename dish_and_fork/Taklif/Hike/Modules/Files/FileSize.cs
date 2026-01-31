using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.Domain
{
    public record FileSize : GreaterThanZero<long>
    {
        public FileSize(long value) : base(value)
        {
        }
        public static implicit operator long(FileSize value) => value.Value;
        public static implicit operator FileSize(long value) => new FileSize(value);
    }
}
