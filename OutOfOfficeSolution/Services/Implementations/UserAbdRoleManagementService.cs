using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;

public class UserAbdRoleManagementService : IUserAndRoleManagementService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly OutOfOfficeSolutionDbContext _dbContext;

    public UserAbdRoleManagementService(IServiceProvider serviceProvider, OutOfOfficeSolutionDbContext outOfOfficeSolutionDbContext)
    {
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        _dbContext = outOfOfficeSolutionDbContext;
    }

    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            return result.Succeeded;
        }
        return false;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        return role != null;
    }

    public async Task<bool> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }
        return false;
    }

    public async Task<bool> CheckUserInRoleAsync(string userName, string role)
    {
        var user = await _dbContext.User
            .Where(u => u.UserName == userName)
            .FirstOrDefaultAsync();

        if (user == null) return false;

        return await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<IList<string>> GetUserRoleAsync(string userName)
    {
        var user = await _dbContext.User
            .Where(u => u.UserName == userName)
            .FirstOrDefaultAsync();

        return await _userManager.GetRolesAsync(user);
    }
}
