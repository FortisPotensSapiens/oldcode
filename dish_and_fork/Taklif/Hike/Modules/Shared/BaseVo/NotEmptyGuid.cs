namespace Daf.SharedModule.Domain.BaseVo
{
    public record NotEmptyGuid : ValueObject<Guid>
    {
        public NotEmptyGuid(Guid value) : base(value)
        {
            if (Value == Guid.Empty)
                throw new ApplicationException("Empty guid!");
        }

        public static implicit operator Guid(NotEmptyGuid value) => value is null ? Guid.Empty : value.Value;
        public static implicit operator NotEmptyGuid(Guid value) => value == default ? null : new NotEmptyGuid(value);
    }
}
