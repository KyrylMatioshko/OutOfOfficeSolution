using Microsoft.AspNetCore.Identity;
using OutOfOfficeSolution.DTO;
using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<int?> GetEmployeeIdByUserNameAsync(string userName);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee?> GetEmployeeByIdAsync(int? employeeId);
        Task<User?> GetUserByUserNameAsync(string userName);
        Task<Employee?> GetEmployeeDetailsAsync(int employeeId);
        Task<IEnumerable<Employee>> GetEmployeesForCurrentUserAsync(IList<string>? currentUserRoles, int? currentUserId);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> SwitchStatusAsync(Employee employee);
        Task<IEnumerable<HrEmployeeDto>> GetListOfHrAsync(UserManager<User> userManager);
        Task<IEnumerable<PmEmployeeDto>> GetListOfPmAsync(UserManager<User> userManager);

    }
}
