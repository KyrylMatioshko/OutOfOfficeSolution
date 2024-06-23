using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequest?> GetLeaveRequestByIdAsync(int leaveRequestId);
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsByUserNameAsync(string userName);
        Task<bool> CreateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task<bool> UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
    }
}
