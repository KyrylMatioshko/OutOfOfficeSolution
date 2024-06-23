namespace OutOfOfficeSolution.Models;

public partial class Project
{
    public int Id { get; set; }

    public string ProjectType { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? ProjectManagerId { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? ProjectManager { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
