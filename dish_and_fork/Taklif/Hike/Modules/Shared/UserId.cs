using Daf.SharedModule.Domain.BaseVo;

namespace Daf.SharedModule.Domain
{
    public record UserId : NotNullOrWhitespaceString
    {
        public UserId(string value) : base(value)
        {
        }

        public static implicit operator string(UserId value) => value is null ? null : value.Value;
        public static implicit operator UserId(string value) => string.IsNullOrWhiteSpace(value) ? null : new UserId(value);
    }

}

