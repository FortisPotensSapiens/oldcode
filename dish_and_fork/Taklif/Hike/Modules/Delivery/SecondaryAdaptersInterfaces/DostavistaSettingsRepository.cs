using Daf.DeliveryModule.SecondaryAdaptersInterfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.SecondaryAdapters.Repositories
{
    public class DostavistaSettingsRepository : IDostavistaSettingsRepository
    {
        private readonly IConfiguration _configuration;

        public DostavistaSettingsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCallBackToken()
        {
            return _configuration["Dostavista:CallBackToken"];
        }
    }
}
