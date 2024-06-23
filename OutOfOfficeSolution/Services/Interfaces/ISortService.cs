using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface ISortService
    {
        public IEnumerable<Employee> SortEmployeeBy(IEnumerable<Employee> employees, string option, string direction);

        public IEnumerable<Project> SortProjectsBy(IEnumerable<Project> projects, string option, string direction);

        IEnumerable<LeaveRequest> SortLeaveRequestBy(IEnumerable<LeaveRequest> leaveRequests, string option, string direction);

    }
}
