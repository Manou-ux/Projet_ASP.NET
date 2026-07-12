using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GESTION_S_E.ViewComponents
{
    public class NotificationsViewComponent : ViewComponent
    {
        private readonly MonDbContext _context;

        public NotificationsViewComponent(MonDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
                return Content(string.Empty);

            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
                return Content(string.Empty);

            int unreadCount = await _context.Notifications
                .Where(n => n.IdUtilisateur == user.IdUtilisateur && !n.Lu)
                .CountAsync();

            var recentNotifications = await _context.Notifications
                .Where(n => n.IdUtilisateur == user.IdUtilisateur)
                .OrderByDescending(n => n.DateEnvoi)
                .Take(10)
                .Select(n => new { n.IdNotification, n.Message, n.DateEnvoi, n.Lu, n.UrlLien })
                .ToListAsync();

            ViewBag.UnreadCount = unreadCount;
            ViewBag.RecentNotifications = recentNotifications;
            return View();
        }
    }
}