using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record OrderId : NotEmptyGuid
    {
        public OrderId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(OrderId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator OrderId(Guid value) => value == default ? null : new OrderId(value);
    }

}

