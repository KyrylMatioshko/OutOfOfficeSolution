using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Enums.ApprovalRequest;
using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Implementations;
using OutOfOfficeSolution.Services.Interfaces;
using OutOfOfficeSolution.ViewModels;

namespace OutOfOfficeSolution.Controllers
{
    [Authorize(Roles = "Admin, HR, PM")]
    public class ApprovalRequestController : Controller
    {
        private readonly IApprovalRequestService _approvalRequestService;
        private readonly IUserAndRoleManagementService _userAndRoleManagementService;
        private readonly IMapper _mapper;

        public ApprovalRequestController(IApprovalRequestService approvalRequestService, IUserAndRoleManagementService userAndRoleManagementService, IMapper mapper)
        {
            _approvalRequestService = approvalRequestService;
            _userAndRoleManagementService = userAndRoleManagementService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";
            var userRoles = await _userAndRoleManagementService.GetUserRoleAsync(currentUserName);
            IEnumerable<ApprovalRequest> approvalRequests = [];

            if (userRoles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
            {
                approvalRequests = await _approvalRequestService.GetAllApprovalRequestsAsync();
            }
            else if (userRoles.Contains("PM", StringComparer.OrdinalIgnoreCase))
            {
                approvalRequests = await _approvalRequestService.GetAllApprovalRequestsByUserNameAsync(currentUserName, "PM");
            }
            else if (userRoles.Contains("HR", StringComparer.OrdinalIgnoreCase))
            {
                approvalRequests = await _approvalRequestService.GetAllApprovalRequestsByUserNameAsync(currentUserName, "HR");
            }

            var approvalRequestsViewModel = _mapper.Map<IEnumerable<ApprovalRequestViewModel>>(approvalRequests);

            return View(approvalRequestsViewModel);
        }

        public async Task<IActionResult> Update(int approvalRequestId, string status)
        {
            _ = await _approvalRequestService.UpdateApprovalRequestAsync(approvalRequestId, status);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int approvalRequestId, [FromServices] IEmployeeService employeeService)
        {
            var approvalRequest = await _approvalRequestService
                .GetApprovalRequestByIdAsync(approvalRequestId);

            if(approvalRequest == null) { return NotFound(); }

            var approver = await employeeService
                .GetEmployeeByIdAsync(approvalRequest.ApproverId);

            if (approvalRequest == null)
                return NotFound();

            var approvalRequestViewModel = _mapper.Map<ApprovalRequestViewModel>(approvalRequest);
            approvalRequestViewModel.Approver = approver;

            return View(approvalRequestViewModel);
        }

        public void InitSelectListsForForm()
        {
            ViewBag.Directions = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Options = Enum.GetValues(typeof(ApprovalRequestOptionEnum)).Cast<ApprovalRequestOptionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

        }

    }
}
