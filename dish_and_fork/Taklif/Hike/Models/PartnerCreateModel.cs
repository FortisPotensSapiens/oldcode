using Hike.Attributes;
using Hike.Entities;
using Hike.Extensions;
using Infrastructure.Entities;

namespace Hike.Models
{
    public class PartnerCreateModel
    {
        /// <summary>
        /// Краткое наименование компании или ФИО ИП/Самозанятого
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        [Required]
        public string Inn { get; set; }
        /// <summary>
        /// Контактный телефон
        /// </summary>
        [PhoneNumberValidation]
        [Required]
        public string ContactPhone { get; set; }
        /// <summary>
        /// Контактный email
        /// </summary>
        [Required]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Тип партнера
        /// </summary>
        public PartnerType Type { get; set; }

        /// <summary>
        /// Код подтверждения из СМС
        /// </summary>
        [Required]
        public string PhoneComfinmationCode { get; set; }
        /// <summary>
        /// Принял ли партнер условия лицензионного соглащения
        /// </summary>
        [Range(typeof(bool), "true", "true")]
        public bool AcceptedTermsOfService { get; set; }

        /// <summary>
        /// Адрес регистрации Юр Лица
        /// </summary>
        [Required]
        public AddressCreateModel RegistrationAddress { get; set; }

        public PartnerDto ToPartnerDto(string directorId)
        {
            var id = Guid.NewGuid();
            var p = new PartnerDto()
            {
                Id = id,
                Title = Title,
                NormalizedTitle = Title.GetNormalizedName(),
                Inn = Inn?.Trim(),
                ContactPhone = ContactPhone,
                ContactEmail = ContactEmail,
                Type = Type,
                State = PartnerState.Created,
                Employes = { new PartnerUser() { PartnerId = id, UserId = directorId, Position = UserPosition.Director } },
                RegistrationAddress = RegistrationAddress.ToAddress(),
                IsPickupEnabled = true,
            };
            return p;
        }
    }
}
