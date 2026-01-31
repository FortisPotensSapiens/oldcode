using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record ApplicationEntityId : NotEmptyGuid
    {
        public ApplicationEntityId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(ApplicationEntityId value) =>  value is null ? Guid.Empty : value.Value;
        public static implicit operator ApplicationEntityId(Guid value) => value == default ? null : new ApplicationEntityId(value);
    }

}

