using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле не заповнене")]
        [MaxLength(255, ErrorMessage = "Максимум 255 символів")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле не заповнене")]
        [MinLength(8, ErrorMessage = "Пароль має складатися мінімум з 8 символів")]
        [MaxLength(20, ErrorMessage = "Максимум 20 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string? Password { get; set; }

        [UIHint("Checkbox")]
        [Display(Name = "Запам'ятати акаунт")]
        public bool RememberMe { get; set; }
    }

}
