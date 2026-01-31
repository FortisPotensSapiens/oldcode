using System.Threading.Tasks;
using Daf.FilesModule.SecondaryAdaptersInterfaces;
using Hike.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Hike.Modules.AdminSettings
{
    public class AdminSettingsRepository : IAdminSettingsRepository
    {
        private readonly IFileSystemClient _files;
        private readonly IDistributedCache _cache;

        public AdminSettingsRepository(IFileSystemClient files,
            IDistributedCache cache)
        {
            _files = files;
            _cache = cache;
        }

        public async Task<AdminSettingsDto> Get()
        {
            var old = await _cache.GetStringAsync(nameof(AdminSettingsDto));
            if (!string.IsNullOrWhiteSpace(old))
                return old.ToObject<AdminSettingsDto>();
            await using var data = await _files.GetAdminSettingsJson();
            var str = await data.GetUtf8Text();
            if (!string.IsNullOrWhiteSpace(str))
                await _cache.SetStringAsync(nameof(AdminSettingsDto), str);
            return string.IsNullOrWhiteSpace(str) ? new AdminSettingsDto { DeliveryCoefficient = 0, MinDeliveryPrice = 0, MerchCoefficient = 0, MinMerchPrice = 0 } : str.ToObject<AdminSettingsDto>();
        }

        public async Task Set(AdminSettingsDto settings)
        {
            var str = settings.ToJson();
            await _cache.SetStringAsync(nameof(AdminSettingsDto), str);
            await _files.SetAdminSettingsJson(str.ToMemoryStream());
        }
    }
}
