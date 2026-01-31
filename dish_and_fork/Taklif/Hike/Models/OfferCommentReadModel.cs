using Daf.MessagingModule.Domain;
using Daf.SharedModule.Domain;
using Hike.Entities;

namespace Hike.Models
{
    public class OfferCommentReadModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        /// <summary>
        /// Идентификатор оффера на заявку к котором относиться коментарий
        /// </summary>
        public Guid OfferId { get; set; }
        /// <summary>
        /// Идентификатор профиля пользователя что оставил комментарий.
        /// </summary>
        public string UserProfileId { get; set; }

        public UserPorfileShortInfoModel UserProfile { get; set; }
        /// <summary>
        /// Идентификатор родительского коментария (на какой коментарий этот является ответом как в Телеграм)
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Дата создания коментария
        /// </summary>
        public DateTime Created { get; set; }

        public OfferCommentReadModel()
        {

        }

        public OfferCommentReadModel(OfferCommentAddedWebSocketMessage entity, UserId userId)
        {
            Id = entity.CommentId;
            Text = entity.Text;
            OfferId = entity.OfferId;
            UserProfileId = userId;
        }

        public static OfferCommentReadModel From(ChatMessageDto dto)
        {
            if (dto == null)
                return null;
            return new OfferCommentReadModel
            {
                Id = dto.Id,
                Text = dto.Text,
                OfferId = dto.OfferId,
                UserProfileId = dto.UserProfileId,
                ParentId = dto.ParentId,
                Created = dto.Created,
                UserProfile = dto?.UserProfile == null ? null : new UserPorfileShortInfoModel { Id = dto.UserProfileId, UserName = dto.UserProfile?.UserName }
            };
        }
    }
}
