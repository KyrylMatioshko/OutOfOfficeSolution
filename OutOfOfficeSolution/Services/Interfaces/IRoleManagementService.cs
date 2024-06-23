namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IRoleManagementService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
    }
}
