using Hike.Extensions;

namespace Daf.SharedModule.Domain.BaseVo
{
    public record NotDefaultDateTime : ValueObject<DateTime>
    {
        public NotDefaultDateTime(DateTime value) : base(value)
        {
            if (value == default)
                new { value }.ThrowApplicationException("Укажите значение для даты и времени!");
        }

        public static implicit operator DateTime(NotDefaultDateTime value) => value is null ? default : value.Value;
        public static implicit operator NotDefaultDateTime(DateTime value) => value == default ? null : new NotDefaultDateTime(value);
    }

}

