using Daf.MessagingModule.Domain;
using Daf.SharedModule.Domain;
using Hike.Entities;
using Hike.Modules.Shared.SecondaryAdapters;

namespace Daf.MessagingModule.SecondaryAdaptersInterfaces
{
    public interface IChatMessagesRepository : IBaseRepository<ChatMessage, ChatMessageId, ChatMessageDto>
    {

    }
}
