using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.ViewModels;

namespace OutOfOfficeSolution.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManageController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly OutOfOfficeSolutionDbContext _context;

        public RoleManageController(UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            OutOfOfficeSolutionDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();

            var users = await _userManager.Users.Include(u => u.Employee).ToListAsync();

            var usersViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel();

                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email ?? "";
                thisViewModel.FullName = user.Employee.FullName;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);

                usersViewModel.Add(thisViewModel);
            }

            ViewBag.Roles = allRoles;

            return View(usersViewModel);
        }

        [HttpPost]
        [Route("RoleManage/ChangeRole/{userId}/{newRoleName}")]
        public async Task<IActionResult> ChangeRole(int userId, string newRoleName)
        {
            var user = await _userManager.Users.Include(u => u.Employee).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return BadRequest();

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any(cr => cr == newRoleName))
                return BadRequest();

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
                return BadRequest();

            var addResult = await _userManager.AddToRoleAsync(user, newRoleName);

            if (!addResult.Succeeded)
                return BadRequest();

            return await AdminRoleCheck();
        }

        public async Task<IActionResult> AdminRoleCheck()
        {
            var currentUserName = User?.Identity?.Name ?? "";

            var currentUser = await _userManager.FindByNameAsync(currentUserName);

            if (currentUser == null) return BadRequest();

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            if (currentUserRoles.Any(cr => cr != "Admin"))
                return RedirectToAction("Logout", "Account");

            return Json(new { success = true });
        }
    }
}
