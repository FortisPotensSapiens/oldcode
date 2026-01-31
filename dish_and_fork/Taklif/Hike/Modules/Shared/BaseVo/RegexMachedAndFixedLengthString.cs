using System.Text.RegularExpressions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record RegexMachedAndFixedLengthString : NotNullOrWhitespaceString
    {
        public RegexMachedAndFixedLengthString(string value, string pattern, int min, int max) : base(value)
        {
            if (!Regex.IsMatch(value, pattern))
                throw new ApplicationException("Строка не соотвествует паттерну!");
            if (value.Length < min)
                throw new ApplicationException("Строка слишком короткая!");
            if (value.Length > max)
                throw new ApplicationException("Строка слишком длинная!");
        }
    }
}
