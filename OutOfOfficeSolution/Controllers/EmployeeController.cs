using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Interfaces;
using OutOfOfficeSolution.ViewModels;
using OutOfOfficeSolution.Enums.Employee;
using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.ViewModels.FilterViewModels;


namespace OutOfOfficeSolution.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin, HR, PM")]
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IUserAndRoleManagementService userAndRoleManagementService, [FromServices] IFilterService filterService,
            [FromServices]ISortService sortService, EmployeeFilterViewModel employeeFilterViewModel)
        {
            var currentUserName = User?.Identity?.Name ?? "";

            int? employeeId = await _employeeService.GetEmployeeIdByUserNameAsync(currentUserName);

            var userRoles = await userAndRoleManagementService.GetUserRoleAsync(currentUserName);

            if (userRoles == null || employeeId == null)
                return NotFound();

            var employees = await _employeeService.GetEmployeesForCurrentUserAsync(userRoles, employeeId);

            var filteredEmployees = filterService.Filter(employees, employeeFilterViewModel.Status, employeeFilterViewModel.Subdivision, employeeFilterViewModel.Position);
            
            if (!(String.IsNullOrEmpty(employeeFilterViewModel.SearchString!)))
            filteredEmployees = filteredEmployees.Where(e => e.FullName.Contains(employeeFilterViewModel.SearchString!, StringComparison.OrdinalIgnoreCase));

            var sortedEmployees = sortService.SortEmployeeBy(filteredEmployees, employeeFilterViewModel.Option!, employeeFilterViewModel.Direction!);

            InitSelectListsForForm();
            InitCurrentFilterParams(employeeFilterViewModel.SearchString!, employeeFilterViewModel.Subdivision!, employeeFilterViewModel.Status!, employeeFilterViewModel.Position!);
            InitCurrentSortParams(employeeFilterViewModel.Option!, employeeFilterViewModel.Direction!);

            var employeesViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(sortedEmployees);

            return View(employeesViewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpGet]
        public async Task<IActionResult> Details(int employeeId)
        {
            var employee = await _employeeService.GetEmployeeDetailsAsync(employeeId);

            if (employee == null) return NotFound();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            return View(employeeViewModel);
        }

        [Authorize(Roles = "Admin, HR")]
        [HttpGet]
        public async Task<IActionResult> Edit(int employeeId, [FromServices] UserManager<User> userManager)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (employee == null) return NotFound();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            var listOfHr = await _employeeService.GetListOfHrAsync(userManager);
            ViewBag.ListOfHr = listOfHr.ToList();

            InitSelectListsForForm();

            return View(employeeViewModel);
        }

        [Authorize(Roles = "Admin, HR")]
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel, [FromServices] UserManager<User> userManager)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(employeeViewModel);
                await _employeeService.UpdateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }

            InitSelectListsForForm();

            var listOfHr = await _employeeService.GetListOfHrAsync(userManager);
            ViewBag.ListOfHr = listOfHr.ToList();

            return View(employeeViewModel);
        }

        [Authorize(Roles = "Admin, HR")]
        [HttpPost]
        public async Task<IActionResult> SwitchStatus(int employeeId)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);

            if (employee == null) return NotFound();

            var result = await _employeeService.SwitchStatusAsync(employee);

            return Json(new { success = result });
        }

        public void InitSelectListsForForm()
        {
            ViewBag.Directions = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Options = Enum.GetValues(typeof(EmployeeOptionEnum)).Cast<EmployeeOptionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Positions = Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Subdivisions = Enum.GetValues(typeof(SubdivisionEnum)).Cast<SubdivisionEnum>().Select(p => new SelectListItem
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

        public void InitCurrentFilterParams(string searchString, string  subdivision, string status,string position)
        {
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSubdivision = subdivision;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPosition = position;
        }
    }
}
