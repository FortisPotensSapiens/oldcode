using Daf.MessagingModule.Domain;
using Daf.SharedModule.Domain;
using Daf.SharedModule.SecondaryAdaptersInterfaces.Repositories;
using Hike.Entities;
using Hike.Modules.Shared.SecondaryAdapters;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface IDevicesRepository : IBaseRepository<Device, DeviceId, DeviceDto>
    {

    }
}
