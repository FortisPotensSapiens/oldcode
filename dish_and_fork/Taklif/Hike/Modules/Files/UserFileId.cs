using Daf.SharedModule.Domain.BaseVo;

namespace Daf.FilesModule.Domain
{
    public record UserFileId : NotEmptyGuid
    {
        public UserFileId(Guid value) : base(value)
        {
        }

        public static implicit operator Guid(UserFileId value) => value.Value;
        public static implicit operator UserFileId(Guid value) => new UserFileId(value);
    }
}
