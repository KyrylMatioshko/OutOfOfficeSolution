using OutOfOfficeSolution.Enums.LeaveRequest;
using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class FilterService : IFilterService
    {
        public IEnumerable<Employee> Filter(IEnumerable<Employee> employees,string? status, string? subdivision, string? position)
        {

            if (!(String.IsNullOrEmpty(status)))
                employees = employees.Where(e => e.Status == status);
            if(!(String.IsNullOrEmpty(subdivision)))
                employees = employees.Where(e => e.Subdivision == subdivision);
            if (!(String.IsNullOrEmpty(position)))
                employees = employees.Where(e => e.Position == position);

            return employees;
        }

        public IEnumerable<Project> Filter(IEnumerable<Project> projects, string? status, string? projectType)
        {

            if (!(String.IsNullOrEmpty(status)))
                projects = projects.Where(p => p.Status == status);
            if (!(String.IsNullOrEmpty(projectType)))
                projects = projects.Where(p => p.ProjectType == projectType);

            return projects;
        }

        public IEnumerable<LeaveRequest> Filter(IEnumerable<LeaveRequest> leaveRequests, string? status)
        {
            if (String.IsNullOrEmpty(status))
                leaveRequests = [];
            else if (status.Equals("New", StringComparison.OrdinalIgnoreCase))
                leaveRequests = leaveRequests.Where(lr => lr.Status == "Новий");
            else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                leaveRequests = leaveRequests.Where(lr => lr.Status == "Відхилено");
            else if (status.Equals("Submited", StringComparison.OrdinalIgnoreCase))
                leaveRequests = leaveRequests.Where(lr => lr.Status == "Затверджено");

            return leaveRequests;
        }

        public IEnumerable<ApprovalRequest> Filter(IEnumerable<ApprovalRequest> approvalRequests, string? status)
        {
            if (String.IsNullOrEmpty(status))
                approvalRequests = [];
            else if (status.Equals("New", StringComparison.OrdinalIgnoreCase))
                approvalRequests = approvalRequests.Where(lr => lr.Status == "Новий");
            else if (status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                approvalRequests = approvalRequests.Where(lr => lr.Status == "Відхилено");
            else if (status.Equals("Submited", StringComparison.OrdinalIgnoreCase))
                approvalRequests = approvalRequests.Where(lr => lr.Status == "Затверджено");

            return approvalRequests;
        }
    }
}
