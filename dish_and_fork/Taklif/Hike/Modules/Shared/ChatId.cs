using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record ChatId : NotEmptyGuid
    {
        public ChatId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(ChatId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator ChatId(Guid value) => value == default ? null : new ChatId(value);
    }

}

