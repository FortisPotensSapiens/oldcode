namespace Hike.Models
{
    public class UserProfileUpdateModel
    {
        [Required(ErrorMessage = "Укажите псевдоним!")]
        [Display(Name = "Псевдоним/Логин/Ник нейм")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Псевдоним должен быть от 1 до 100 символов длинной")]
        [RegularExpression(@"^[^\s]*$", ErrorMessage = "Пробелы запрещены!")]
        public string UserName { get; set; }
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
    }
}
