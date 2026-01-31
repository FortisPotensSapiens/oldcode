using Daf.SharedModule.Domain;
using Daf.SharedModule.Domain.BaseVo;

namespace Daf.MessagingModule.Domain
{
    public record ChatMessage(
    ChatMessageId Id,
    ChatId ChatId,
    LongText Text,
   UserId UserId,
   ChatMessageId? ParentId,
   NotDefaultDateTime Created
    ) : IEntity<ChatMessageId>;
}
