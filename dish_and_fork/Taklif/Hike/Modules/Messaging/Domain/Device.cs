using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{

    public record Device(DeviceId Id, PushToken Token, UserId UserId) : IEntity<DeviceId>
    {

    }
}
