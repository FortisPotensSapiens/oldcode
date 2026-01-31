using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record OfferId : NotEmptyGuid
    {
        public OfferId(Guid value) : base(value)
        {
        }
        public static implicit operator Guid(OfferId value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator OfferId(Guid value) => value == default ? null : new OfferId(value);
        ChatId ToChatId() => new ChatId(Value);
    }

}

