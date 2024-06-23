using Microsoft.AspNetCore.Identity;
using OutOfOfficeSolution.Services.Interfaces;

public class RoleManagementService : IRoleManagementService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleManagementService(IServiceProvider serviceProvider)
    {
        _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
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
}
