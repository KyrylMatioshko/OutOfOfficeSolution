using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.DTO;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly OutOfOfficeSolutionDbContext _dbContext;

        public EmployeeService(OutOfOfficeSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int?> GetEmployeeIdByUserNameAsync(string userName)
        {
            int? employeeId = await _dbContext.User
                .Where(u => u.UserName == userName)
                .Include(u => u.Employee)
                .Select(e => e.Employee.Id)
                .FirstOrDefaultAsync();

            return employeeId;
        }
        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _dbContext.Employees.FindAsync(employeeId);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int? employeeId)
        {
            return await _dbContext.Employees.FindAsync(employeeId);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            var user = await _dbContext.User
                .Include(u => u.Employee)
                .ThenInclude(e => e.Projects)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return user;
        }
        public async Task<Employee?> GetEmployeeDetailsAsync(int employeeId)
        {
            return await _dbContext.Employees
                .Include(e => e.PeoplePartnerNavigation)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForCurrentUserAsync(IList<string>? currentUserRoles, int? currentUserId)
        {
            if (currentUserRoles!.Contains("HR", StringComparer.OrdinalIgnoreCase))
            {
                return await _dbContext.Employees.Where(e => e.PeoplePartner == currentUserId).ToListAsync();
            }
            else if (currentUserRoles!.Contains("PM", StringComparer.OrdinalIgnoreCase))
            {
                return await _dbContext.Employees
                    .Include(e => e.Projects)
                    .Where(e => e.Projects.Any(p => p.ProjectManagerId == currentUserId)).ToListAsync();
            }

            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }
        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SwitchStatusAsync(Employee employee)
        {
            employee.Status = employee.Status == "Active" ? "InActive" : "Active";

            _dbContext.Entry(employee).Property(e => e.Status).IsModified = true;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<HrEmployeeDto>> GetListOfHrAsync(UserManager<User> userManager)
        {
            var usersInHrRole = await userManager.GetUsersInRoleAsync("HR");

            var ListOfHr = usersInHrRole.Join(
                            _dbContext.Employees,
                            u => u.EmployeeId,
                            e => e.Id,
                            (u, e) => new HrEmployeeDto
                            {
                                Id = e.Id,
                                FullName = e.FullName
                            });

            return ListOfHr;
        }

        public async Task<IEnumerable<PmEmployeeDto>> GetListOfPmAsync(UserManager<User> userManager)
        {
            var userInPmRole = await userManager.GetUsersInRoleAsync("PM");

            var ListOfPm = userInPmRole.Join(
                            _dbContext.Employees,
                            u => u.EmployeeId,
                            e => e.Id,
                            (u, e) => new PmEmployeeDto
                            {
                                Id = e.Id,
                                FullName = e.FullName
                            });

            return ListOfPm;
        }
    }
}
