using System.Threading.Tasks;
using Daf.MessagingModule.Domain;
using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.PrimaryAdapters
{

    public interface IMessaginService
    {
        Task SendNotificationMessage(NotificatonsMessage message);
        Task<DeviceId> AddDevice(UserId userId, PushToken pushToken);
        Task<Device?> Get(DeviceId id);
        Task<List<Device>> Get(PushToken token);
        Task Update(Device device);
        Task Delete(DeviceId id);
        Task CheckTimer(string key);
        Task<bool> CheckCodeIsValid(string code, PhoneNumber phone);
        Task<List<ChatMessage>> GetByChatId(ChatId id);
        Task<ChatMessageId> Create(ChatId chatId, ChatMessageId? parentId, LongText text, UserId userId);
        Task<List<Device>> Get(PageNumber pageNumber, PageSize pageSize, UserId userId);
        Task<BigCount> CountDevices(UserId userId);
    }
}
