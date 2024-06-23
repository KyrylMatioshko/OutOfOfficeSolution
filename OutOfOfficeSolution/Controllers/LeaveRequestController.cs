using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Enums.LeaveRequest;
using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Implementations;
using OutOfOfficeSolution.Services.Interfaces;
using OutOfOfficeSolution.ViewModels;
using OutOfOfficeSolution.ViewModels.FilterViewModels;

namespace OutOfOfficeSolution.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public LeaveRequestController(ILeaveRequestService leaveRequestService, IEmployeeService employeeService, IMapper mapper)
        {
            _leaveRequestService = leaveRequestService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(LeaveRequestFilterViewModel leaveRequestFilterViewModel,
            [FromServices] IFilterService filterService, [FromServices] ISortService sortService)
        {
            InitSelectListsForForm();
            InitCurrentFilterParams(leaveRequestFilterViewModel.Status!);
            InitCurrentSortParams(leaveRequestFilterViewModel.Option!, leaveRequestFilterViewModel.Direction!);


            var currentUserName = User?.Identity?.Name ?? "";

            var leaveRequests = await _leaveRequestService.GetAllLeaveRequestsByUserNameAsync(currentUserName);

            if (!(leaveRequestFilterViewModel.IsForFiltering))
            {
                var lRequestViewModel = _mapper.Map<IEnumerable<LeaveRequestViewModel>>(leaveRequests);

                return View(lRequestViewModel);
            }
            var filteredLeaveRequests = filterService.Filter(leaveRequests, leaveRequestFilterViewModel.Status);

            var sortedLeaveRequests = sortService.SortLeaveRequestBy(filteredLeaveRequests, leaveRequestFilterViewModel.Option!, leaveRequestFilterViewModel.Direction!);

            var leaveRequestsViewModel = _mapper.Map<IEnumerable<LeaveRequestViewModel>>(sortedLeaveRequests);

            return View(leaveRequestsViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int leaveRequestId)
        {
            InitSelectListsForForm();

            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);

            var leaveRequestViewModel = _mapper.Map<LeaveRequestViewModel>(leaveRequest);

            return View(leaveRequestViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.NameOfView = nameof(Index);

            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";

            int? employeeId = await _employeeService.GetEmployeeIdByUserNameAsync(currentUserName);

            return View(new LeaveRequestViewModel { EmployeeId = employeeId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestViewModel leaveRequestsViewModel)
        {
            if (ModelState.IsValid)
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestsViewModel);

                var createdResult = await _leaveRequestService.CreateLeaveRequestAsync(leaveRequest);

                if (!createdResult) return BadRequest();

                return RedirectToAction(nameof(Index));
            }

            InitSelectListsForForm();
            return View(leaveRequestsViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int leaveRequestId)
        {
            InitSelectListsForForm();

            var leaveRequest = await _leaveRequestService.GetLeaveRequestByIdAsync(leaveRequestId);

            var leaveRequestViewModel = _mapper.Map<LeaveRequestViewModel>(leaveRequest);

            return View(leaveRequestViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(LeaveRequestViewModel leaveRequestsViewModel)
        {
            if (ModelState.IsValid)
            {
                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestsViewModel);

                var updatedResult = await _leaveRequestService.UpdateLeaveRequestAsync(leaveRequest);

                if (!updatedResult) return BadRequest();

                return RedirectToAction(nameof(Index));
            }

            InitSelectListsForForm();

            return View(leaveRequestsViewModel);
        }
        public void InitSelectListsForForm()
        {

            ViewBag.Directions = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Options = Enum.GetValues(typeof(LeaveRequestOptionEnum)).Cast<LeaveRequestOptionEnum>().Select(p => new SelectListItem
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

        public void InitCurrentSortParams(string option, string direction)
        {
            ViewBag.CurrentOption = option;
            ViewBag.CurrentDirection = direction;
        }
        public void InitCurrentFilterParams(string status)
        {
            ViewBag.CurrentStatus = status;
        }
    }
}
