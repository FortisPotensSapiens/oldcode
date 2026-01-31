using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.Domain
{
    public record PushToken : ShortText
    {
        public PushToken(string value) : base(value)
        {
        }
        public static implicit operator string(PushToken value) => value is null ? null : value.Value;
        public static implicit operator PushToken(string value) => string.IsNullOrWhiteSpace(value) ? null : new PushToken(value);
    }
}
