using Daf.SharedModule.Domain.BaseVo;
using Hike.Attributes;
using Hike.Extensions;

namespace Daf.SharedModule.Domain
{
    public record PhoneNumber : NotNullOrWhitespaceString
    {
        public PhoneNumber(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ApplicationException("Укажите телефон");
            if (!new PhoneNumberValidationAttribute().IsValid(value))
                new { value }.ThrowApplicationException("Не валидный номер телефона");
        }

        public static implicit operator string(PhoneNumber value) => value is null ? null : value.Value;
        public static implicit operator PhoneNumber(string value) => string.IsNullOrWhiteSpace(value) ? null : new PhoneNumber(value);
    }

}

