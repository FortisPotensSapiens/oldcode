using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.Domain
{
    public record OfferCommentAddedWebSocketMessage(
    OfferId OfferId,
    LongText Text,
    ChatMessageId CommentId
    ) : WebSocketMessage
    { }
}
