using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Enums.Project;
using OutOfOfficeSolution.Enums.SortEnums;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Services.Interfaces;
using OutOfOfficeSolution.ViewModels;
using OutOfOfficeSolution.ViewModels.FilterViewModels;




namespace OutOfOfficeSolution.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        public ProjectController(IProjectService projectService, IMapper mapper, IEmployeeService employeeService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IUserAndRoleManagementService userAndRoleManagementService,
            [FromServices] ISortService sortService, [FromServices] IFilterService filterService, ProjectFilterViewModel projectFilterViewModel)
        {
            InitSelectListsForForm();
            InitCurrentFilterParams(projectFilterViewModel.ProjectType!, projectFilterViewModel.Status!);
            InitCurrentSortParams(projectFilterViewModel.Option!, projectFilterViewModel.Direction!);

            var currentUserName = User?.Identity?.Name ?? "";

            int? employeeId = await _employeeService.GetEmployeeIdByUserNameAsync(currentUserName);

            var userRoles = await userAndRoleManagementService.GetUserRoleAsync(currentUserName);

            if (userRoles == null || employeeId == null)
                return NotFound();

            var projects = await _projectService.GetProjectsForCurrentUserAsync(userRoles, employeeId);

            var employeesAfterFilter = filterService.Filter(projects, projectFilterViewModel.Status, projectFilterViewModel.ProjectType).ToList();

            var sortedProjects = sortService.SortProjectsBy(employeesAfterFilter, projectFilterViewModel.Option!, projectFilterViewModel.Direction!);

            var projectViewModel = _mapper.Map<List<ProjectViewModel>>(sortedProjects);

            ViewBag.NameOfView = nameof(Index);
            ViewBag.listOfEmployee = await _employeeService.GetAllEmployeesAsync();

            return View(projectViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EmployeeProjects([FromServices] ISortService sortService, 
            [FromServices] IFilterService filterService, ProjectFilterViewModel projectFilterViewModel)
        {
            InitSelectListsForForm();
            InitCurrentFilterParams(projectFilterViewModel.ProjectType!, projectFilterViewModel.Status!);
            InitCurrentSortParams(projectFilterViewModel.Option!, projectFilterViewModel.Direction!);

            string? userName = User?.Identity?.Name ?? "";

            var user = await _employeeService.GetUserByUserNameAsync(userName);

            if (user == null) return BadRequest();

            var employeeProjects = user.Employee.Projects.AsEnumerable();

            var employeesAfterFilter = filterService.Filter(employeeProjects, projectFilterViewModel.Status, projectFilterViewModel.ProjectType);

            var sortedProjects = sortService.SortProjectsBy(employeesAfterFilter, projectFilterViewModel.Option!, projectFilterViewModel.Direction!);

            var employeeProjectsViewModel = _mapper.Map<List<ProjectViewModel>>(sortedProjects);

            ViewBag.NameOfView = nameof(EmployeeProjects);

            return View(employeeProjectsViewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpGet]
        public async Task<IActionResult> Create([FromServices] UserManager<User> userManager,
            [FromServices] IUserAndRoleManagementService userAndRoleManagementService)
        {
            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";

            var userRoles = await userAndRoleManagementService.GetUserRoleAsync(currentUserName);

            if (userRoles.Contains("PM", StringComparer.OrdinalIgnoreCase))
                ViewBag.CurrentPmId = await _employeeService.GetEmployeeIdByUserNameAsync(currentUserName);
            else
            {
                var ListOfPm = await _employeeService.GetListOfPmAsync(userManager);
                ViewBag.ListOfPm = ListOfPm.ToList();
            }

            var employees = await _employeeService.GetAllEmployeesAsync();

            var newProject = new ProjectViewModel() { ListOfEmployee = employees };

            return View(newProject);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpPost]
        public async Task<IActionResult> Create(ProjectViewModel newProjectViewModel,
            [FromServices] UserManager<User> userManager,
            [FromServices] IUserAndRoleManagementService userAndRoleManagementService)
        {
            if (ModelState.IsValid)
            {
                if (newProjectViewModel.SelectedEmployeeIds != null && newProjectViewModel.SelectedEmployeeIds.Any())
                {
                    var project = _mapper.Map<Project>(newProjectViewModel);
                    var createdResult = await _projectService.CreateProjectWithEmployeesAsync(newProjectViewModel.SelectedEmployeeIds, project);

                    if (!createdResult) return BadRequest();

                    return RedirectToAction(nameof(Index));
                }

                var result = await _projectService.CreateProjectAsync(_mapper.Map<Project>(newProjectViewModel));
                return RedirectToAction(nameof(Index));
            }

            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";

            var userRoles = await userAndRoleManagementService.GetUserRoleAsync(currentUserName);

            if (userRoles.Contains("PM", StringComparer.OrdinalIgnoreCase))
                ViewBag.CurrentPmId = await _employeeService.GetEmployeeIdByUserNameAsync(currentUserName);
            else
            {
                var ListOfPm = await _employeeService.GetListOfPmAsync(userManager);
                ViewBag.ListOfPm = ListOfPm.ToList();
            }

            var employees = await _employeeService.GetAllEmployeesAsync();

            newProjectViewModel.ListOfEmployee = employees;

            return View(newProjectViewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpGet]
        public async Task<IActionResult> Edit(int projectId,
            [FromServices] UserManager<User> userManager)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);

            if (project == null) return NotFound();

            var projectViewModel = _mapper.Map<ProjectViewModel>(project);

            InitSelectListsForForm();

            var ListOfPm = await _employeeService.GetListOfPmAsync(userManager);

            ViewBag.ListOfPm = ListOfPm.ToList();

            return View(projectViewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProjectViewModel viewModel,
            [FromServices] UserManager<User> userManager)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<Project>(viewModel);
                await _projectService.UpdateProjectAsync(model);
                return RedirectToAction(nameof(Index));
            }

            InitSelectListsForForm();

            var ListOfPm = await _employeeService.GetListOfPmAsync(userManager);

            ViewBag.ListOfPm = ListOfPm.ToList();

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpPost]
        public async Task<IActionResult> SwitchStatus(int projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null) return BadRequest();

            var result = await _projectService.SwitchStatusAsync(project);

            return Json(new { success = result });
        }

        [Authorize(Roles = "Admin, HR, PM")]
        [HttpGet]
        public async Task<IActionResult> Details(int projectId)
        {
            var project = await _projectService.GetProjectDetailsAsync(projectId);

            if (project == null) return NotFound();

            var projectViewModel = _mapper.Map<ProjectViewModel>(project);

            return View(projectViewModel);
        }

        [Authorize(Roles = "Admin, PM")]
        [HttpPost]
        public async Task<IActionResult> AddEmployeeToProject(int employeeId, int projectId)
        {
            _ = await _projectService.AddEmployeeToProjectAsync(projectId, employeeId);

            return RedirectToAction(nameof(Index));
        }

        public void InitSelectListsForForm()
        {
            ViewBag.Options = Enum.GetValues(typeof(ProjectOptionEnum)).Cast<ProjectOptionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();

            ViewBag.Directions = Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>().Select(p => new SelectListItem
            {
                Value = p.ToString(),
                Text = p.ToString()
            }).ToList();


            ViewBag.ProjectTypes = Enum.GetValues(typeof(ProjectTypeEnum)).Cast<ProjectTypeEnum>().Select(p => new SelectListItem
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
        public void InitCurrentFilterParams(string projectType, string status)
        {
            ViewBag.CurrentProjectType = projectType;
            ViewBag.CurrentStatus = status;
        }
    }
}
