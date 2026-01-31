
using Microsoft.AspNetCore.Identity;

namespace Hike.Entities
{
    public class HikeUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public HikeUser()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUser"/>.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        /// The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public HikeUser(string userName) : this()
        {
            UserName = userName;
        }
        public List<OrderDto> OrdersAsBuyer { get; set; } = new List<OrderDto>();
        public List<PartnerUser> Partners { get; set; } = new List<PartnerUser>();
        public List<OfferToApplicationDto> Offers { get; set; } = new List<OfferToApplicationDto>();
        public List<ApplicationDto> Applications { get; set; } = new List<ApplicationDto>();
        public List<ChatMessageDto> Comments { get; set; } = new List<ChatMessageDto>();
        public List<RatingDto> Ratings { get; set; } = new();
        public List<DeviceDto> Devices { get; set; } = new();
        public List<UserFileDto> UserFiles { get; set; } = new();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Пользователь принял согласие на рассылку 
        /// </summary>
        public bool AcceptedConsentToMailings { get; set; }
        /// <summary>
        /// Пользователь согласился на обработку его персональных данных
        /// </summary>
        public bool AcceptedConsentToPersonalDataProcessing { get; set; }
        /// <summary>
        /// Пользователь принял политику конфеденциальности
        /// </summary>
        public bool AcceptedPivacyPolicy { get; set; }
        /// <summary>
        /// Пользователь принял условия использования
        /// </summary>
        public bool AcceptedTermsOfUse { get; set; }
        /// <summary>
        /// Пользователь принял оферту для физических лиц 
        /// </summary>
        public bool AcceptedOfferFoUser { get; set; }
        //public void Applay(UserEntity entity)
        //{
        //    UserName = entity.UserName;
        //    NormalizedUserName = UserName?.GetNormalizedName();
        //    Email = entity.Email;
        //    NormalizedEmail = Email?.GetNormalizedName();
        //    PhoneNumber = entity.PhoneNumber;
        //    EmailConfirmed = entity.EmailConfirmed;
        //    PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
        //    PasswordHash = entity.PasswordHash;
        //}

        //public UserEntity ToUser() => new UserEntity(
        //    Id,
        //    Email, PhoneNumber, UserName, EmailConfirmed,
        //    PhoneNumberConfirmed, AccessFailedCount, LockoutEnabled, LockoutEnd, PasswordHash,
        //    Partners.Select(x => new Daf.SharedModule.Domain.PartnerId(x.PartnerId)).ToList(),
        //    OrdersAsBuyer.Select(x => (OrderId)x.Id).ToList(),
        //    Offers.Select(x => (OfferId)x.Id).ToList(),
        //    Applications.Select(x => (ApplicationEntityId)x.Id).ToList(),
        //    Comments.Select(x => (ChatMessageId)x.Id).ToList(),
        //    Ratings.Select(x => (RatingEntityId)x.Id).ToList(),
        //    Devices.Select(x => (DeviceId)x.Id).ToList(),
        //    UserFiles.Select(x => (FileId)x.FileId),
        //    string.IsNullOrWhiteSpace(PasswordHash)
        //    );

        //public HikeUser(UserEntity entity)
        //{
        //    Id = entity.Id;
        //    UserName = entity.UserName;
        //    NormalizedUserName = UserName?.GetNormalizedName();
        //    Email = entity.Email;
        //    NormalizedEmail = Email?.GetNormalizedName();
        //    PhoneNumber = entity.PhoneNumber;
        //    EmailConfirmed = entity.EmailConfirmed;
        //    PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
        //    PasswordHash = entity.PasswordHash;
        //}
    }
}
