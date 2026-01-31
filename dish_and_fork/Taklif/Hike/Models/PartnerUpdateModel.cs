using Hike.Attributes;
using Hike.Entities;
using Hike.Extensions;

namespace Hike.Models
{
    public class PartnerUpdateModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Краткое название компании или ФИО ИП/Самозанятого
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Описание компании
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Логотип или фото самозанятого (из FileDto)
        /// </summary>
        public Guid? ImageId { get; set; }

        [Required]
        public PeriodModel WorkingTime { get; set; } = new PeriodModel();

        [Required]
        [MinLength(1)]
        [MaxLength(7)]
        public List<DayOfWeek> WorkingDays { get; set; } = new List<DayOfWeek>();

        [Required]
        public AddressCreateModel Address { get; set; }

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
        [EmailAddress]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Код подтверждения из СМС. Нужен только если меняется номер телефона
        /// </summary>
        public string? PhoneComfinmationCode { get; set; }

        /// <summary>
        /// Адрес регистрации Юр Лица
        /// </summary>
        [Required]
        public AddressCreateModel RegistrationAddress { get; set; }

        /// <summary>
        /// Есть ли самовывоз?
        /// </summary>
        [Required]
        public bool IsPickupEnabled { get; set; }

        public void Map(PartnerDto partner)
        {
            if (WorkingTime != null)
                if (WorkingTime.Start.Hour > WorkingTime.End.Hour)
                    new { model = this }.ThrowApplicationException("Время начала работы больше времени конца работы");
            partner.Title = Title;
            partner.WorkingDays = WorkingDays;
            partner.ImageId = ImageId;
            partner.Description = Description;
            partner.WorkingTime = WorkingTime?.DeepClone();
            partner.Inn = Inn;
            partner.ContactPhone = ContactPhone;
            partner.ContactEmail = ContactEmail;
            partner.IsPickupEnabled = IsPickupEnabled;
        }
    }
}
