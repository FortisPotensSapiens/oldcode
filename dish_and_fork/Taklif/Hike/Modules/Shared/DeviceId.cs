using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record DeviceId : NotDefault<Guid>
    {
        public DeviceId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(DeviceId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator DeviceId(Guid value) => value == default ? null : new DeviceId(value);
    }

}

