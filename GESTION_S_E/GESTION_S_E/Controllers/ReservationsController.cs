using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly MonDbContext _context;

        public ReservationsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                .Include(r => r.Club)
                .OrderByDescending(r => r.DateReservation)
                .ToListAsync();
            return View(reservations);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Salles = await _context.Salles.Where(s => s.Disponible).ToListAsync();
            ViewBag.Utilisateurs = await _context.Utilisateurs.Where(u => u.Actif).ToListAsync();
            ViewBag.Clubs = await _context.Clubs.Where(c => c.Actif).ToListAsync();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationSalle reservation)
        {
            if (string.IsNullOrEmpty(reservation.Motif))
                reservation.Motif = null;
            if (reservation.IdClub == 0)
                reservation.IdClub = null;
            
            if (ModelState.IsValid)
            {
                reservation.Statut = "en_attente";
                reservation.DateReservation = DateTime.UtcNow;
                _context.ReservationsSalles.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation ajoutée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Salles = await _context.Salles.Where(s => s.Disponible).ToListAsync();
            ViewBag.Utilisateurs = await _context.Utilisateurs.Where(u => u.Actif).ToListAsync();
            ViewBag.Clubs = await _context.Clubs.Where(c => c.Actif).ToListAsync();
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewBag.Salles = await _context.Salles.ToListAsync();
            ViewBag.Utilisateurs = await _context.Utilisateurs.ToListAsync();
            ViewBag.Clubs = await _context.Clubs.ToListAsync();
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationSalle reservation)
        {
            if (id != reservation.IdReservation)
            {
                return NotFound();
            }

            ModelState.Remove("Salle");
            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Réservation modifiée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ReservationsSalles.Any(r => r.IdReservation == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            
            ViewBag.Salles = await _context.Salles.ToListAsync();
            ViewBag.Utilisateurs = await _context.Utilisateurs.ToListAsync();
            ViewBag.Clubs = await _context.Clubs.ToListAsync();
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                .Include(r => r.Club)
                .FirstOrDefaultAsync(r => r.IdReservation == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation != null)
            {
                _context.ReservationsSalles.Remove(reservation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation supprimée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                .Include(r => r.Club)
                .FirstOrDefaultAsync(r => r.IdReservation == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Valider/5
        [HttpPost, ActionName("Valider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Valider(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation != null)
            {
                reservation.Statut = "validee";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation validée !";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Annuler/5
        [HttpPost, ActionName("Annuler")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Annuler(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation != null)
            {
                reservation.Statut = "annulee";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation annulée !";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}