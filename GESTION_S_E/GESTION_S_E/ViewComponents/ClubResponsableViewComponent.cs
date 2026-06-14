using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.ViewComponents
{
    public class ClubResponsableViewComponent : ViewComponent
    {
        private readonly MonDbContext _context;

        public ClubResponsableViewComponent(MonDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
                return Content("");

            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
                return Content("");

            // Vérifier si l'étudiant est responsable d'au moins un club
            bool estResponsable = await _context.Clubs.AnyAsync(c => c.IdResponsable == user.IdUtilisateur);

            return View(estResponsable);
        }
    }
}