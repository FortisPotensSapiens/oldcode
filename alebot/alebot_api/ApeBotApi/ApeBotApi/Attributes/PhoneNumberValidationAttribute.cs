using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using PhoneNumbers;

namespace ApeBotApi.Attributes
{
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        public PhoneNumberValidationAttribute()
        {
            ErrorMessage = "Не валидный номер телефона";
        }

        public override bool IsValid(object value)
        {
            var str = value?.ToString();

            if (string.IsNullOrWhiteSpace(str))
                return true;
            if (!Regex.IsMatch(str, @"^\+\d+$"))
                return false;
            return IsValidPhone(str);
        }

        private bool IsValidPhone(string phone)
        {
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var phoneNumber = phoneNumberUtil.Parse(phone, "ZZ");
                return phoneNumberUtil.IsValidNumber(phoneNumber);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }
}
