namespace Daf.SharedModule.Domain.BaseVo
{
    public record FixedLengthString : NotNullOrWhitespaceString
    {
        public FixedLengthString(string value, int min, int max) : base(value)
        {
            if (value.Length < min)
                throw new ApplicationException("Строка слишком короткая!");
            if (value.Length > max)
                throw new ApplicationException("Строка слишком длинная!");
        }
    }
}
