namespace Hike.Models
{
    public class PasswordUpdateModel : PasswordCreateModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }
    }
}
