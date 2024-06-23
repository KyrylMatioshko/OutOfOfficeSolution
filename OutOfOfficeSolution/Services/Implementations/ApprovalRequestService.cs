using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class ApprovalRequestService : IApprovalRequestService
    {
        private readonly OutOfOfficeSolutionDbContext _dbContext;

        public ApprovalRequestService(OutOfOfficeSolutionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApprovalRequest?> GetApprovalRequestByIdAsync(int approvalRequestId)
        {
            return await _dbContext.ApprovalRequests
                .Include(ar => ar.LeaveRequest)
                .ThenInclude(lr => lr!.Employee)
                .FirstOrDefaultAsync(ar => ar.Id == approvalRequestId);
        }

        public async Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync()
        {
            return await _dbContext.ApprovalRequests.ToListAsync();
        }

        public async Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsByUserNameAsync(string userName, string role)
        {
            IEnumerable<ApprovalRequest> approvalRequests = [];

            int? employeeId = await _dbContext.User
                    .Where(ea => ea.UserName == userName)
                    .Include(ea => ea.Employee)
                    .Select(ea => ea.Employee.Id)
                    .FirstOrDefaultAsync();

            if (string.Equals("PM", role, StringComparison.OrdinalIgnoreCase))
            {
                approvalRequests = await _dbContext.ApprovalRequests
                                .Include(ar => ar.LeaveRequest)
                                .ThenInclude(lr => lr!.Employee)
                                .ThenInclude(e => e!.Projects)
                                .Where(ar => ar.LeaveRequest!.EmployeeId != employeeId
                                             && ar.LeaveRequest!.Employee!.Projects
                                             .Any(p => p.ProjectManagerId == employeeId))
                                .ToListAsync();
            }
            else if (string.Equals("HR", role, StringComparison.OrdinalIgnoreCase))
            {
                approvalRequests = await _dbContext.ApprovalRequests
                    .Where(ar => ar.ApproverId == employeeId)
                    .Include(ar => ar.LeaveRequest)
                    .ToListAsync();
            }

            return approvalRequests;
        }

        public async Task<bool> UpdateApprovalRequestAsync(int approvalRequestId, string status)
        {
            var approvalRequest = await _dbContext.ApprovalRequests
                 .Where(ar => ar.Id == approvalRequestId)
                 .Include(ar => ar.LeaveRequest)
                 .FirstOrDefaultAsync();

            if(approvalRequest == null) { return false; }

            if (string.Equals(status, "submit", StringComparison.OrdinalIgnoreCase))
            {
                approvalRequest.Status = "Затверджено";
                approvalRequest.LeaveRequest!.Status = approvalRequest.Status;
            }
            else
            {
                approvalRequest.Status = "Відхилено";
                approvalRequest.LeaveRequest!.Status = approvalRequest.Status;
            }

            _dbContext.ApprovalRequests.Update(approvalRequest);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
