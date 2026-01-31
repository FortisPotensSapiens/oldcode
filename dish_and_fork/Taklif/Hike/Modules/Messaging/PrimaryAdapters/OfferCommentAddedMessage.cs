using Daf.SharedModule.Domain;

namespace Daf.MessagingModule.PrimaryAdapters
{
    public record OfferCommentAddedMessage(
        OfferId OfferId,
        LongText Text,
        ChatMessageId CommentId,
        IEnumerable<UserId> UserIds
        ) : NotificatonsMessage
    { }
}
