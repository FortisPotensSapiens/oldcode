namespace Daf.SharedModule.Domain.BaseVo
{
    public record NotEmptyCollection<T> : ValueObject<IReadOnlyCollection<T>>
    {
        public NotEmptyCollection(ICollection<T> values) : base(values.ToList().AsReadOnly())
        {
            if (values is null)
                throw new ApplicationException("Пустая коллекция!");
            if (values.Count == 0)
                throw new ApplicationException("Пустая коллекция!");
        }

        public static implicit operator List<T>(NotEmptyCollection<T> value) => value is null ? new() : value.Value.ToList();
        public static implicit operator NotEmptyCollection<T>(List<T> value) => value is null || value.Count == 0 ? null : new NotEmptyCollection<T>((ICollection<T>)value);
        public static implicit operator T[](NotEmptyCollection<T> value) => value is null ? new T[0] : value.Value.ToArray();
        public static implicit operator NotEmptyCollection<T>(T[] value) => value is null || value.Length == 0 ? null : new NotEmptyCollection<T>((ICollection<T>)value);
    }

}

