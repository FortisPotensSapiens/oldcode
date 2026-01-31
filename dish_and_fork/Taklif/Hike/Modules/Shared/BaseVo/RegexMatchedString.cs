using System.Text.RegularExpressions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record RegexMatchedString : NotNullOrWhitespaceString
    {
        public RegexMatchedString(string value, string pattern) : base(value)
        {
            if (!Regex.IsMatch(value, pattern))
                throw new ApplicationException("Строка не соотвествует паттерну!");
        }
    }
}
