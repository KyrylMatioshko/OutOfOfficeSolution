namespace OutOfOfficeSolution.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Subdivision { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? PeoplePartner { get; set; }

    public int? OutOfOfficeBalance { get; set; }

    public string? Photo { get; set; }

    public virtual ICollection<ApprovalRequest> ApprovalRequests { get; set; } = new List<ApprovalRequest>();

    public virtual ICollection<Employee> InversePeoplePartnerNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual Employee? PeoplePartnerNavigation { get; set; }

    public virtual ICollection<Project> ProjectsNavigation { get; set; } = new List<Project>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

public partial class Employee
{
    public User User { get; set; } = null!;
}
