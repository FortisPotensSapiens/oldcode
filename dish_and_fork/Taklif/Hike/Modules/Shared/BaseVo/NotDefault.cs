namespace Daf.SharedModule.Domain.BaseVo
{
    public record NotDefault<T> : ValueObject<T>
    {

        public NotDefault(T value) : base(value)
        {
            if (value is null)
                throw new ApplicationException("Укажите значение!");
            T d = default;
            if (value.Equals(d))
                throw new ApplicationException("Укажите значение!");
            if (value is 0)
                throw new ApplicationException("Укажите значение!");
        }
        public static implicit operator T(NotDefault<T> value) => value is null ? default : value.Value;
        public static implicit operator NotDefault<T>(T value) => value is null || default(T).Equals(value) ? null : new NotDefault<T>(value);
    }

}

