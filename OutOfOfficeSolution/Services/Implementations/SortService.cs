using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class SortService : ISortService
    {
        public IEnumerable<Employee> SortEmployeeBy(IEnumerable<Employee> employees, string option,string direction)
        {
            switch (option)
            {
                case nameof(EmployeeOptionEnum.FullName):
                    if (direction == nameof(DirectionEnum.Ascending))
                        employees = employees.OrderBy(e => e.FullName).ThenBy(e => e.OutOfOfficeBalance);
                    else
                        employees = employees.OrderByDescending(e => e.FullName).ThenByDescending(e => e.OutOfOfficeBalance);
                    break;

                case nameof(EmployeeOptionEnum.OutOfOfficeBalance):
                    if (direction == nameof(DirectionEnum.Ascending)) 
                        employees = employees.OrderBy(e => e.OutOfOfficeBalance).ThenBy(e => e.FullName);
                    else
                        employees = employees.OrderByDescending(e => e.OutOfOfficeBalance).ThenByDescending(e => e.FullName);
                    break;

                default:
                    if (direction == nameof(DirectionEnum.Ascending))
                        employees = employees.OrderBy(e => e.FullName);
                    break;
            }

            return employees;
        }

        public IEnumerable<Project> SortProjectsBy(IEnumerable<Project> projects, string option, string direction)
        {
            switch (option)
            {
                case nameof(ProjectOptionEnum.StartDate):
                    if (direction == nameof(DirectionEnum.Ascending))
                        projects = projects.OrderBy(p => p.StartDate).ThenBy(p => p.EndDate);
                    else
                        projects = projects.OrderByDescending(p => p.StartDate).ThenByDescending(p => p.EndDate);
                    break;

                case nameof(ProjectOptionEnum.EndDate):
                    if (direction == nameof(DirectionEnum.Ascending))
                        projects = projects.OrderBy(p => p.EndDate).ThenBy(p => p.StartDate);
                    else
                        projects = projects.OrderByDescending(p => p.EndDate).ThenByDescending(p => p.StartDate);
                    break;

                default:
                    if (direction == nameof(DirectionEnum.Ascending))
                        projects = projects.OrderByDescending(p => p.EndDate);
                    break;
            }

            return projects;
        }

        public IEnumerable<LeaveRequest> SortLeaveRequestBy(IEnumerable<LeaveRequest> leaveRequests, string option, string direction)
        {
            switch (option)
            {
                case nameof(LeaveRequestOptionEnum.EndDate):
                    if (direction == nameof(DirectionEnum.Ascending))
                        leaveRequests = leaveRequests.OrderBy(lr => lr.EndDate).ThenBy(lr => lr.StartDate);
                    else
                        leaveRequests = leaveRequests.OrderByDescending(lr => lr.EndDate).ThenBy(lr => lr.StartDate);
                    break;
                default:
                    if (direction == nameof(DirectionEnum.Ascending))
                        leaveRequests = leaveRequests.OrderBy(lr => lr.StartDate).ThenBy(lr => lr.EndDate);
                    else
                        leaveRequests = leaveRequests.OrderByDescending(lr => lr.StartDate).ThenBy(lr => lr.EndDate);
                    break;
            }

            return leaveRequests;
        }
    }
}
