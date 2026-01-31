using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record FileId : NotEmptyGuid
    {
        public FileId(Guid value) : base(value)
        {
        }

        public static implicit operator Guid(FileId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator FileId(Guid value) => value == default ? null : new FileId(value);
    }

}

