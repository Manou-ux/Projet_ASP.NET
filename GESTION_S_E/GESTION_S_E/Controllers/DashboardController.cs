using Microsoft.AspNetCore.Mvc;

namespace ProjetAsp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult EnseignantDashboard()
        {
            return View();
        }

        public IActionResult EleveDashboard()
        {
            return View();
        }
    }
}