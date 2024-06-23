using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Project?> GetProjectByIdAsync(int projectId);
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<Project>> GetProjectsForCurrentUserAsync(IList<string>? currentUserRoles, int? currentUserId);
        Task<bool> CreateProjectAsync(Project project);
        Task<bool> UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<Project?> GetProjectDetailsAsync(int projectId);
        Task<bool> CreateProjectWithEmployeesAsync(IEnumerable<int> ids,Project project);
        Task<bool> SwitchStatusAsync(Project project);
        Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId);
    }
}
