namespace Daf.SharedModule.Domain
{
    public interface IEntity<T>
    {
        T Id { get; }
    }

}

