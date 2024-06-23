namespace OutOfOfficeSolution.Models;

public partial class ApprovalRequest
{
    public int Id { get; set; }

    public int? ApproverId { get; set; }

    public int? LeaveRequestId { get; set; }

    public string Status { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual Employee? Approver { get; set; }

    public virtual LeaveRequest? LeaveRequest { get; set; }
}
