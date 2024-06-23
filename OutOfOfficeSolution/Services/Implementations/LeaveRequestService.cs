using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;

namespace OutOfOfficeSolution.Services.Implementations
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly OutOfOfficeSolutionDbContext _dbContext;
        private readonly IEmployeeService _employeeService;

        public LeaveRequestService(OutOfOfficeSolutionDbContext dbContext, IEmployeeService employeeService)
        {
            _dbContext = dbContext;
            _employeeService = employeeService;
        }

        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(int leaveRequestId)
        {
            return await _dbContext.LeaveRequests.Where(lr => lr.Id == leaveRequestId)
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsByUserNameAsync(string userName)
        {
            return await _dbContext.User
                    .Where(e => e.UserName == userName)
                    .Include(e => e.Employee)
                    .ThenInclude(e => e.LeaveRequests)
                    .SelectMany(e => e.Employee.LeaveRequests)
                    .Include(e => e.ApprovalRequest)
                    .ToListAsync();
        }

        public async Task<bool> CreateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(leaveRequest.EmployeeId);

            if(employee == null) { return false; }

            var approvalRequest = new ApprovalRequest
            {
                ApproverId = employee.PeoplePartner,
                Comment = leaveRequest.Comment
            };

            leaveRequest.ApprovalRequest = approvalRequest;
            _dbContext.LeaveRequests.Add(leaveRequest);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _dbContext.LeaveRequests.Update(leaveRequest);

            var approvalRequests = await _dbContext.LeaveRequests
                .Where(lr => lr.Id == leaveRequest.Id)
                .Include(lr => lr.ApprovalRequest)
                .FirstOrDefaultAsync();

            if(approvalRequests == null) { return false; }

            var approvalRequest = approvalRequests.ApprovalRequest;

            approvalRequest!.Comment = leaveRequest.Comment;

            _dbContext.ApprovalRequests.Update(approvalRequest);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
