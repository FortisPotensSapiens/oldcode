using Daf.SharedModule.Domain.BaseVo;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record EmailVo : ShortText
    {
        public EmailVo(string value) : base(value)
        {
            if (!new EmailAddressAttribute().IsValid(value))
                new { value }.ThrowApplicationException("Не валидный email");
        }

        public static implicit operator string(EmailVo value) => value is null ? null : value.Value;
        public static implicit operator EmailVo(string value) => string.IsNullOrWhiteSpace(value) ? null : new EmailVo(value);
    }

}

