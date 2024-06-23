namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IUserAndRoleManagementService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> CheckUserInRoleAsync(string userName, string role);
        Task<IList<string>> GetUserRoleAsync(string userName);
    }
}
