using OutOfOfficeSolution.Models;

namespace OutOfOfficeSolution.Services.Interfaces
{
    public interface IApprovalRequestService
    {
        Task<ApprovalRequest?> GetApprovalRequestByIdAsync(int approvalRequestId);
        Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsAsync();
        Task<IEnumerable<ApprovalRequest>> GetAllApprovalRequestsByUserNameAsync(string userName, string role);
        Task<bool> UpdateApprovalRequestAsync(int approvalRequestId, string status);
    }
}
