using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record ChatMessageId : NotEmptyGuid
    {
        public ChatMessageId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(ChatMessageId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator ChatMessageId(Guid value) => value == default ? null : new ChatMessageId(value);
    }

}

