using OutOfOfficeSolution.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Повне ім'я")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Відділ")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        public string Subdivision { get; set; } = null!;

        [Display(Name = "Позиція")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        public string Position { get; set; } = null!;

        [Display(Name = "Статус")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        public string Status { get; set; } = null!;

        [Display(Name = "HR Менеджер")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Значення повинно бути від {1} до {2}.")]
        public int? PeoplePartner { get; set; }

        [Display(Name = "Доступний баланс днів відпустки")]
        [Range(0, 90, ErrorMessage = "Значення повинно бути від {1} до {2}.")]
        public int? OutOfOfficeBalance { get; set; }

        [Display(Name = "Photo")]
        [MaxLength(255)]
        public string? Photo { get; set; }

        public Employee? PeoplePartnerNavigation { get; set; }
    }
}
