using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly OutOfOfficeSolutionDbContext _dbContext;

        public ProjectService(OutOfOfficeSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project?> GetProjectByIdAsync(int projectId)
        {
            return await _dbContext.Projects.FindAsync(projectId);
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsForCurrentUserAsync(IList<string>? currentUserRoles, int? currentUserId)
        {
            if (currentUserRoles!.Contains("PM", StringComparer.OrdinalIgnoreCase))
            {
                return await _dbContext.Projects.Include(p => p.Employees).Where(p => p.ProjectManagerId == currentUserId).ToListAsync();
            }
                
            return await _dbContext.Projects.Include(p => p.Employees).ToListAsync();
        }

        public async Task<Project?> GetProjectDetailsAsync(int projectId)
        {
            return await _dbContext.Projects
                .Include(p => p.Employees)
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<bool> CreateProjectAsync(Project project)
        {
            _dbContext.Projects.Add(project);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            _dbContext.Projects.Update(project);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FindAsync(projectId);
            if (project != null)
            {
                _dbContext.Projects.Remove(project);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }
        public async Task<bool> CreateProjectWithEmployeesAsync(IEnumerable<int> ids, Project project)
        {
            foreach (var id in ids) 
            {
                var employee = await _dbContext.Employees
                    .Include(e => e.Projects)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null) return false;

                if (!(employee.Projects.Contains(project)))
                    employee.Projects.Add(project);
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SwitchStatusAsync(Project project)
        {
            project.Status = project.Status == "Active" ? "InActive" : "Active";

            _dbContext.Entry(project).Property(p => p.Status).IsModified = true;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddEmployeeToProjectAsync(int projectId, int employeeId)
        {
            var project = await _dbContext.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null) return false;
            if (project == null) return false;

            if (project.Employees.Contains(employee)) return false;

            project.Employees.Add(employee);

            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
