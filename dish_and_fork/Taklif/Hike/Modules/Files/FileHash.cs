using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.FilesModule.Domain
{
    public record FileHash : NotEmptyCollection<byte>
    {
        public FileHash(ICollection<byte> values) : base(values)
        {
        }
        public NotNullOrWhitespaceString GetHexString() => Value.ToArray().ToHexString();
        public static implicit operator List<byte>(FileHash value) => value.Value.ToList();
        public static implicit operator FileHash(List<byte> value) => new FileHash((ICollection<byte>)value);
        public static implicit operator byte[](FileHash value) => value.Value.ToArray();
        public static implicit operator FileHash(byte[] value) => new FileHash((ICollection<byte>)value);
    }
}
