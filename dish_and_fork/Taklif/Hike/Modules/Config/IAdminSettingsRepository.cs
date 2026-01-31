using System.Threading.Tasks;

namespace Hike.Modules.AdminSettings
{
    public interface IAdminSettingsRepository
    {
        Task<AdminSettingsDto> Get();
        Task Set(AdminSettingsDto settings);
    }
}
