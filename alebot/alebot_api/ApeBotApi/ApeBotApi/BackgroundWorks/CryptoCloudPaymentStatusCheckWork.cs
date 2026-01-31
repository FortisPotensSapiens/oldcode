using AleBotApi.DbContexts;
using AleBotApi.DbDtos.Enums;
using AleBotApi.Services;
using Microsoft.EntityFrameworkCore;

namespace AleBotApi.BackgroundWorks
{
    public class CryptoCloudWalletPaymentStatusCheck : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CryptoCloudWalletPaymentStatusCheck> _logger;

        public CryptoCloudWalletPaymentStatusCheck(
            IServiceProvider services,
            ILogger<CryptoCloudWalletPaymentStatusCheck> logger
            )
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var sp = scope.ServiceProvider;

                    var accountsService = sp.GetRequiredService<IAccountsService>();

                    await accountsService.CompletePaindTransactionsAsync();
                    await accountsService.CompletePaindOrdersAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error on globan run");
                }

                await Task.Delay(3600_00, stoppingToken);
            }
        }
    }
}
