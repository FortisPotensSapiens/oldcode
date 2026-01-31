using System.Text.RegularExpressions;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule
{
    public partial record Inn : NotNullOrWhitespaceString
    {
        public Inn(string value) : base(value)
        {
            if (value.Length < 10 || value.Length > 12)
                throw new ApplicationException("ИНН должно быть от 10 до 12 едениц длинной!");
            if (!MyRegex().IsMatch(value))
                throw new ApplicationException("ИНН должен состоять только из цифр");
        }

        public static implicit operator string(Inn value) => value is null ? null : value.Value;
        public static implicit operator Inn(string value) => string.IsNullOrWhiteSpace(value) ? null : new Inn(value);

        [GeneratedRegex("^\\d+$")]
        private static partial Regex MyRegex();
    }

}

