using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля до 255 символів")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MinLength(8, ErrorMessage = "Довжина пароля мінімум 8 символів")]
        [MaxLength(255, ErrorMessage = "Довжина паролю максимум 255 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        [DataType(DataType.Text)]
        [Display(Name = "Повне ім'я")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        [Display(Name = "Відділ")]
        public string? Subdivision { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        [Display(Name = "Позиція")]
        public string? Position { get; set; }

        [Range(0, 90, ErrorMessage = "Значення повинно бути від {1} до {2}.")]
        [Display(Name = "OutOfOfficeBalance")]
        public int OutOfOfiiceBalance { get; set; }

        [Display(Name = "PeoplePartner")]
        public int? PeoplePartner { get; set; }

        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        [Display(Name = "Photo")]
        public string? Photo { get; set; }
    }
}
