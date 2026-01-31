using System.Text.RegularExpressions;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public partial record UserName : ShortText
    {
        public UserName(string value) : base(value)
        {
            if (!MyRegex().IsMatch(value))
                throw new ApplicationException("Пробелы запрешены!");
        }

        public static implicit operator string(UserName value) => value is null ? null : value.Value;
        public static implicit operator UserName(string value) => string.IsNullOrWhiteSpace(value) ? null : new UserName(value);

        [GeneratedRegex("^[^\\s]*$")]
        private static partial Regex MyRegex();
    }

}

