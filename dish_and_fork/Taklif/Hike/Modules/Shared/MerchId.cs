using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record MerchId : NotEmptyGuid
    {
        public MerchId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(MerchId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator MerchId(Guid value) => value == default ? null : new MerchId(value);
    }

}

