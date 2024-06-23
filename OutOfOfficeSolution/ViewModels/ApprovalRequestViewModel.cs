using OutOfOfficeSolution.Models;
using System.ComponentModel.DataAnnotations;

namespace OutOfOfficeSolution.ViewModels
{
    public class ApprovalRequestViewModel
    {
        public int Id { get; set; }

        public int? ApproverId { get; set; }

        public int? LeaveRequestId { get; set; }

        [Display(Name = "Статус")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 50 символів")]
        public string Status { get; set; } = null!;

        [Display(Name = "Коментар")]
        [MaxLength(255, ErrorMessage = "Довжина поля не більше 1000 символів")]
        public string? Comment { get; set; }

        public virtual Employee? Approver { get; set; }

        public virtual LeaveRequest? LeaveRequest { get; set; }
    }
}
