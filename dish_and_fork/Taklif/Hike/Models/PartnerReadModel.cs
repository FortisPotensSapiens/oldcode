using Hike.Attributes;
using Hike.Entities;

namespace Hike.Models
{
    public class PartnerReadModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Логотип компании или фото ИП/Самозанятого
        /// </summary>
        public FileReadModel Image { get; set; }

        /// <summary>
        /// Краткое название компании или ФИО ИП/Самозанятого
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Описание компании.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Состояние 
        /// </summary>
        public PartnerState State { get; set; }
        /// <summary>
        /// Тип
        /// </summary>
        public PartnerType Type { get; set; }

        public AddressReadModel Address { get; set; }

        /// <summary>
        /// Часы работы
        /// </summary>
        public PeriodModel WorkingTime { get; set; } = new PeriodModel();

        /// <summary>
        /// ИНН
        /// </summary>
        [Required]
        public string Inn { get; set; }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        [PhoneNumberValidation]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Контактный email
        /// </summary>
        [Required]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Дни по которым работает этот магазин
        /// </summary>
        public List<DayOfWeek> WorkingDays { get; set; } = new List<DayOfWeek>();
        /// <summary>
        /// Идентификатор в платежной системе (в YKassa сейчас)
        /// </summary>
        public string? ExternalId { get; set; }
        /// <summary>
        /// Количество звезд (до 5)
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Есть ли самовывоз?
        /// </summary>
        [Required]
        public bool IsPickupEnabled { get; set; }

        /// <summary>
        /// Адрес регистрации Юр Лица
        /// </summary>
        public AddressReadModel RegistrationAddress { get; set; }

        public static PartnerReadModel From(ShopDto shop, string baseUrl)
        {
            if (shop?.Partner == null)
                return null;
            var dto = shop.Partner;
            return new PartnerReadModel()
            {
                Id = shop.Id,
                Title = dto.Title,
                Description = dto.Description,
                State = dto.State,
                Type = dto.Type,
                Image = FileReadModel.From(dto.Image, baseUrl),
                Address = AddressReadModel.From(dto.Address),
                WorkingTime = PeriodModel.From(dto?.WorkingTime),
                Inn = dto.Inn,
                ContactPhone = dto.ContactPhone,
                ContactEmail = dto.ContactEmail,
                WorkingDays = dto.WorkingDays,
                ExternalId = dto.ExternalId,
                Rating = dto.Rating,
                IsPickupEnabled = dto.IsPickupEnabled,
                RegistrationAddress = AddressReadModel.From(dto.RegistrationAddress)
            };
        }
    }
}
