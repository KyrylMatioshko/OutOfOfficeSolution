using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OutOfOfficeSolution.Enums.Employee;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Interfaces;
using OutOfOfficeSolution.ViewModels;

namespace OutOfOfficeSolution.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin, HR")]
        [HttpGet]
        public async Task<IActionResult> Register([FromServices] IEmployeeService employeeService)
        {
            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";

            var currentUser = await _userManager.GetUserAsync(User!);

            bool isInHrRole = await _userManager.IsInRoleAsync(currentUser!, "HR");

            if (isInHrRole)
                ViewBag.CurrentHrId = await employeeService.GetEmployeeIdByUserNameAsync(currentUserName);
            else
            {
                var listOfHr = await employeeService.GetListOfHrAsync(_userManager);
                ViewBag.ListOfHr = listOfHr.ToList();
            }

            return View();
        }

        [Authorize(Roles = "Admin, HR")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model,
            [FromServices] OutOfOfficeSolutionDbContext context,
            [FromServices] IEmployeeService employeeService)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(e => e.Email == model.Email);

                if (user == null)
                {
                    var employee = new Employee
                    {
                        FullName = model.FullName ?? "",
                        Subdivision = model.Subdivision ?? "",
                        Position = model.Position ?? "",
                        Status = "Active",
                        OutOfOfficeBalance = model.OutOfOfiiceBalance,
                        Photo = model.Photo ?? "",
                        PeoplePartner = model.PeoplePartner
                    };

                    user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        Employee = employee
                    };

                    var result = await _userManager.CreateAsync(user, model.Password ?? "");

                    if (result.Succeeded)
                    {
                        var addedToRole = await _userManager.AddToRoleAsync(user, "Employee");
                        if (!addedToRole.Succeeded) return BadRequest();
                    }
                    else
                        return BadRequest();

                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError(nameof(model.Email), "Користувач з таким Email вже існує");
            }

            InitSelectListsForForm();

            var currentUserName = User?.Identity?.Name ?? "";

            var currentUser = await _userManager.GetUserAsync(User!);

            if (currentUser == null) return NotFound();

            bool isInHrRole = await _userManager.IsInRoleAsync(currentUser!, "HR");

            if (isInHrRole)
                ViewBag.CurrentHrId = await employeeService.GetEmployeeIdByUserNameAsync(currentUserName);
            else
            {
                var listOfHr = await employeeService.GetListOfHrAsync(_userManager);
                ViewBag.ListOfHr = listOfHr.ToList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email ?? "", model.Password ?? "", model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");

                ModelState.AddModelError(nameof(LoginViewModel.Email), "Неправильний Email або пароль");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public void InitSelectListsForForm()
        {
            ViewBag.Positions = Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>().Select(p => new SelectListItem
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
    }
}
