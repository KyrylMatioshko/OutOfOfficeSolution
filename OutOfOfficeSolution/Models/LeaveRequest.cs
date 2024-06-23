namespace OutOfOfficeSolution.Models;

public partial class LeaveRequest
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string AbsenceReason { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual ApprovalRequest? ApprovalRequest { get; set; }

    public virtual Employee? Employee { get; set; }
}
