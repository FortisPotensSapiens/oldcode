global using System;
global using System.Collections.Generic;
global using Hike.Entities.Base;

namespace Hike.Entities
{
    public class ChatMessageDto : EntityDtoBase
    {
        [StringLength(1000)]
        public string Text { get; set; }
        public Guid OfferId { get; set; }
        public OfferToApplicationDto Offer { get; set; }
        [Required]
        public string UserProfileId { get; set; }
        public HikeUser UserProfile { get; set; }
        public Guid? ParentId { get; set; }
        public ChatMessageDto Parent { get; set; }
        public List<ChatMessageDto> Descendants { get; set; } = new List<ChatMessageDto>();

        //public void Applay(ChatMessage entity)
        //{
        //    Text = entity.Text;
        //    OfferId = entity.ChatId;
        //    UserProfileId = entity.UserId;
        //    ParentId = entity.ParentId;
        //}

        //public ChatMessage ToMessage() => new ChatMessage(
        //    Id,
        //    OfferId,
        //    Text,
        //    UserProfileId,
        //    ParentId,
        //    Created
        //    );

        //public ChatMessageDto(ChatMessage entity)
        //{
        //    Id = entity.Id;
        //    Text = entity.Text;
        //    OfferId = entity.ChatId;
        //    UserProfileId = entity.UserId;
        //    ParentId = entity.ParentId;
        //}

        //public ChatMessageDto()
        //{

        //}
    }
}
