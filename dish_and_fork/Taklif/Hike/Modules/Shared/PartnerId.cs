using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record PartnerId : NotEmptyGuid
    {
        public PartnerId(Guid value) : base(value)
        {
        }

        public static implicit operator Guid(PartnerId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator PartnerId(Guid value) => value == default ? null : new PartnerId(value);
    }

}

