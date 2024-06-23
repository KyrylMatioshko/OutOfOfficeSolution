using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OutOfOfficeSolution.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
