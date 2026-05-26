using GESTION_S_E.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GESTION_S_E.Controllers
{
    [Authorize] // Toutes les actions nécessitent une authentification
    public class NotificationsController : Controller
    {
        private readonly MonDbContext _context;

        public NotificationsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Notifications (liste des notifications de l'utilisateur connecté)
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var notifications = await _context.Notifications
                .Where(n => n.IdUtilisateur == userId)
                .OrderByDescending(n => n.DateEnvoi)
                .ToListAsync();
            return View(notifications);
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (notification == null) return NotFound();
            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.IdUtilisateur = GetCurrentUserId();
                notification.DateEnvoi = DateTime.Now;
                notification.Lu = false;
                _context.Add(notification);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Notification ajoutée.";
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (notification == null) return NotFound();
            return View(notification);
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNotification,Message,Lu")] Notification notification)
        {
            if (id != notification.IdNotification) return NotFound();
            var userId = GetCurrentUserId();
            var existing = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (existing == null) return NotFound();

            if (ModelState.IsValid)
            {
                existing.Message = notification.Message;
                existing.Lu = notification.Lu;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Notification modifiée.";
                return RedirectToAction(nameof(Index));
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (notification == null) return NotFound();
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Notification supprimée.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Notifications/MarkAsRead/5
        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.IdNotification == id && n.IdUtilisateur == userId);
            if (notification != null)
            {
                notification.Lu = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Notifications/MarkAllAsRead
        [HttpPost]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = GetCurrentUserId();
            var notifications = await _context.Notifications
                .Where(n => n.IdUtilisateur == userId && !n.Lu)
                .ToListAsync();
            foreach (var n in notifications) n.Lu = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper : récupère l'ID de l'utilisateur connecté (à adapter selon votre système d'auth)
        private int GetCurrentUserId()
        {
            // Exemple avec les claims : vous avez peut-être stocké "UserId" ou "Id"
            var userIdClaim = User.FindFirst("UserId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return 0;
            return int.Parse(userIdClaim);
        }
    }
}