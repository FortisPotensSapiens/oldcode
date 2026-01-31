using Hike.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hike.Models
{
    public sealed class UserProfileDetailsReadModel
    {
        [Obsolete]
        public UserProfileDetailsReadModel()
        {

        }

        public UserProfileDetailsReadModel(
            string id,
            string userName,
            string email,
            bool emailConfirmed,
            bool lockoutEnabled,
            DateTimeOffset? lockoutEnd,
            int accessFailedCount,
            string securityStamp,
            string concurrencyStamp,
            List<string> roles,
            List<UserLoginInfo> logins,
            bool hasPassword,
            IEnumerable<AuthenticationSchemeReadModel> otherLogins,
            bool acceptedConsentToMailings,
            bool acceptedConsentToPersonalDataProcessing,
            bool acceptedPivacyPolicy,
            bool acceptedTermsOfUser,
            bool acceptedOfferFoUser)
        {
            Id = id;
            UserName = userName;
            Email = email;
            EmailConfirmed = emailConfirmed;
            LockoutEnabled = lockoutEnabled;
            LockoutEnd = lockoutEnd;
            AccessFailedCount = accessFailedCount;
            SecurityStamp = securityStamp;
            ConcurrencyStamp = concurrencyStamp;
            Roles = roles;
            Logins = logins;
            HasPassword = hasPassword;
            OtherLogins = otherLogins?.ToList();
            ShowRemoveLoginButton = hasPassword || logins?.Count > 1;
            AcceptedConsentToMailings = acceptedConsentToMailings;
            AcceptedConsentToPersonalDataProcessing = acceptedConsentToPersonalDataProcessing;
            AcceptedPivacyPolicy = acceptedPivacyPolicy;
            AcceptedTermsOfUse = acceptedTermsOfUser;
            AcceptedOfferFoUser = acceptedOfferFoUser;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        /// <summary>
        /// Текущие способы входа которые прикреплены к текущему пользователю (Googel, Facebook)
        /// </summary>
        public List<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>();
        public bool HasPassword { get; set; }
        /// <summary>
        /// Доступные способы входа которые можно прекркпить к текущему пользователю (Googel, Facebook)
        /// </summary>
        public List<AuthenticationSchemeReadModel> OtherLogins { get; set; } = new List<AuthenticationSchemeReadModel>();

        public bool ShowRemoveLoginButton { get; set; }
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

        public static UserProfileDetailsReadModel From(HikeUser dto, IEnumerable<IdentityRole> roles, IEnumerable<UserLoginInfo> logins, bool hasPassword, IEnumerable<AuthenticationSchemeReadModel> otherLogins)
        {
            if (dto == null)
                return null;
            return new UserProfileDetailsReadModel(
                dto.Id,
                dto.UserName,
                dto.Email,
                dto.EmailConfirmed,
                dto.LockoutEnabled,
                dto.LockoutEnd,
                dto.AccessFailedCount,
                dto.SecurityStamp,
                dto.ConcurrencyStamp,
                roles?.Select(x => x.NormalizedName).ToList() ?? new List<string>(),
                logins?.ToList(),
                hasPassword,
                otherLogins,
                dto.AcceptedConsentToMailings,
                dto.AcceptedConsentToPersonalDataProcessing,
                dto.AcceptedPivacyPolicy,
                dto.AcceptedTermsOfUse,
                dto.AcceptedOfferFoUser
            );
        }
    }
}
