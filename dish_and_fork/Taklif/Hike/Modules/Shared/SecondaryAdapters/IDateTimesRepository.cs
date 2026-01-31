namespace Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories
{
    public interface IDateTimesRepository
    {
        DateTime Now();
        DateTime LocalNow();
    }

    public class DateTimesRepository : IDateTimesRepository
    {
        public DateTime LocalNow()
        {
            return DateTime.UtcNow.AddHours(4);
        }

        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
