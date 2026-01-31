namespace Daf.SharedModule.Domain.BaseVo
{
    public record NotNullOrWhitespaceString : ValueObject<string>
    {
        public NotNullOrWhitespaceString(string value) : base(value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ApplicationException("String is null or white space!");
        }

        public static implicit operator string(NotNullOrWhitespaceString value) => value is null ? null : value.Value;
        public static implicit operator NotNullOrWhitespaceString(string value) => string.IsNullOrWhiteSpace(value) ? null : new NotNullOrWhitespaceString(value);
    }
}
