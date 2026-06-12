using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Task.Controllers
{
    [Authorize(Roles = "Admin") ]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Display dashboard with summary statistics
            return View();
        }
    }
}
