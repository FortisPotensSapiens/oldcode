using System.ComponentModel.DataAnnotations;
using ApeBotApi.Attributes;
namespace AleBotApi.Models
{
    public class UserRegistrationModel
    {
        /// <summary>
        /// Адресс почты на которую регается пользователь
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Email { get; set; }

        /// <summary>
        /// Пароль который хочет поставить себе пользователь
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 4)]
        public string Password { get; set; }

        /// <summary>
        /// ID Реферера - человека по чьей реферальной ссылке пользователь пришел на сайт.
        /// </summary>
        public Guid? RefererId { get; set; }

        /// <summary>
        /// Телефон пользователя с + и кодом страны аля +9929992222
        /// </summary>
        [PhoneNumberValidation]
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Параметры которы были в url регистраци. UTM теги, id реферера и прочее.
        /// </summary>
        public string? RegistrationQueryParams { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [StringLength(100)]
        public string? FullName { get; set; }
    }
}
