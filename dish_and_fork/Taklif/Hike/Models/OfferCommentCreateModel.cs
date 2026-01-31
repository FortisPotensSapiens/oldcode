using Hike.Entities;

namespace Hike.Models
{
    /// <summary>
    /// Коментарий к предложению
    /// </summary>
    public class OfferCommentCreateModel
    {
        /// <summary>
        /// Текст коментария
        /// </summary>
        [Required]
        public string Text { get; set; }
        /// <summary>
        /// Идентификатор предложения на заявку к которой оставлен коментарий
        /// </summary>
        public Guid OfferId { get; set; }
        /// <summary>
        /// Коментарий на который является ответом данный комментарий. Как в Телеграм где можно на сообщений отвечать.
        /// </summary>
        public Guid? ParentId { get; set; }

        public ChatMessageDto ToComment(string profileId) => new ChatMessageDto
        {
            Id = Guid.NewGuid(),
            Text = Text,
            OfferId = OfferId,
            UserProfileId = profileId,
            ParentId = ParentId
        };
    }
}
