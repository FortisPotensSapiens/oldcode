using System.Threading;
using Microsoft.Extensions.Hosting;

namespace Hike.Clients
{
    public class CancellationTokensRepository : ICancellationTokensRepository
    {
        private readonly IHostApplicationLifetime _lifetime;

        public CancellationTokensRepository(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }
        public CancellationToken GetDefault()
        {
            return _lifetime.ApplicationStopping;
        }
    }
}
