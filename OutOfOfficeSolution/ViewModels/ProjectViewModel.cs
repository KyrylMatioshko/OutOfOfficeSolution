using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 255 символів")]
        [DataType(DataType.Text)]
        [Display(Name = "Тип проєкта")]
        public string ProjectType { get; set; } = null!;

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата початку")]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата завершення")]
        public DateOnly? EndDate { get; set; }

        [Display(Name = "PM Id")]
        public int? ProjectManagerId { get; set; }

        [StringLength(1000, ErrorMessage = "Довжина коментаря не може перевищувати 1000 символів")]
        [DataType(DataType.Text)]
        [Display(Name = "Додаткова інформація")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове для заповнення")]
        [MaxLength(50, ErrorMessage = "Довжина поля не більше 50 символів")]
        [Display(Name = "Статус")]
        public string Status { get; set; } = null!;

        public virtual Employee? ProjectManager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public IEnumerable<int>? SelectedEmployeeIds { get; set; }
        public IEnumerable<Employee>? ListOfEmployee { get; set; }
    }
}
