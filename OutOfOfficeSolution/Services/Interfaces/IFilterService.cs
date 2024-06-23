using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IFilterService
    {
        IEnumerable<Employee> Filter(IEnumerable<Employee> employees, string? status, string? subdivision, string? position);
        IEnumerable<Project> Filter(IEnumerable<Project> projects, string? status, string? projectType);
        IEnumerable<LeaveRequest> Filter(IEnumerable<LeaveRequest> leaveRequests, string? status);
    }
}
