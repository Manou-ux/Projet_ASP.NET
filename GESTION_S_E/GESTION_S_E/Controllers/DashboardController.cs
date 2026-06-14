using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;
using System.Linq;
using System.Threading.Tasks;

namespace GESTION_S_E.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly MonDbContext _context;

        public DashboardController(MonDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null) return Challenge();

            // Préparer les données communes
            ViewBag.TotalSalles = await _context.Salles.CountAsync();
            ViewBag.TotalClasses = await _context.Classes.CountAsync();
            ViewBag.TotalMatieres = await _context.Matieres.CountAsync();
            ViewBag.TotalReservations = await _context.ReservationsSalles.CountAsync();
            ViewBag.TotalClubs = await _context.Clubs.CountAsync();

            switch (user.Role?.ToLower())
            {
                case "scolarite":
                    return View("ScolariteDashboard");
                case "enseignant":
                    return View("EnseignantDashboard");
                case "eleve":
                    return View("EleveDashboard");
                default:
                    return View("Index");
            }
        }
    }
}