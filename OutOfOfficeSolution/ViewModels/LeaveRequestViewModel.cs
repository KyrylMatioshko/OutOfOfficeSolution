using OutOfOfficeSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class LeaveRequestViewModel
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        [Display(Name = "Причина відпустки")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        public string AbsenceReason { get; set; } = null!;

        [Display(Name = "Дата початку відпустки")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Display(Name = "Дата кінця відпустки")]
        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Коментар")]
        [MaxLength(1000, ErrorMessage = "Довжина поля не більше 1000 символів")]
        public string? Comment { get; set; }

        [Display(Name = "Статус")]
        public string? Status { get; set; }
        public Employee? Employee { get; set; }
    }
}
