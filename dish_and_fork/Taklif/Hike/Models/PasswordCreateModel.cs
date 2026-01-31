namespace Hike.Models
{
    public class PasswordCreateModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} должен быть минимум {2} и максимум {1} символов длинной.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
