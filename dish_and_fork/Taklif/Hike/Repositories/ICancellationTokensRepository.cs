using System.Threading;

namespace Hike.Clients
{
    public interface ICancellationTokensRepository
    {
        CancellationToken GetDefault();
    }
}
