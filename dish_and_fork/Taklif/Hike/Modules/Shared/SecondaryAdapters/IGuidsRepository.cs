namespace Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories
{
    public interface IGuidsRepository
    {
        Guid GetNew();
    }

    public class GuidsRepository : IGuidsRepository
    {
        public Guid GetNew()
        {
            return Guid.NewGuid();
        }
    }
}
