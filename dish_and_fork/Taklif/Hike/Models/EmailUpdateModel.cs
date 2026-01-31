namespace Hike.Models
{
    public class EmailUpdateModel
    {
        [Required(ErrorMessage = "Укажитеж адрес")]
        [EmailAddress(ErrorMessage = "Не валидный адресс")]
        [Display(Name = "Новый email")]
        public string NewEmail { get; set; }
    }
}
