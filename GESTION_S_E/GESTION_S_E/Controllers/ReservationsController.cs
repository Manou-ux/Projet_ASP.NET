using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

// GET: Reservations/Calendar
public async Task<IActionResult> Calendar()
{
    try
    {
        var reservations = await _context.ReservationsSalles
            .Include(r => r.Salle)
            .Include(r => r.Utilisateur)
            .Include(r => r.Club)
            .OrderBy(r => r.DateReservation)
            .ToListAsync();
        return View(reservations);
    }
    catch (Exception ex)
    {
        TempData["Error"] = "Erreur de chargement du calendrier : " + ex.Message;
        return View(new List<ReservationSalle>());
    }
}

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Salles = new SelectList(await _context.Salles.ToListAsync(), "IdSalle", "NomSalle");
            ViewBag.Utilisateurs = new SelectList(await _context.Utilisateurs.ToListAsync(), "IdUtilisateur", "Email");
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "IdClub", "NomClub");
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationSalle reservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    reservation.Statut = "en_attente";
                    reservation.DateReservation = DateTime.UtcNow;
                    _context.ReservationsSalles.Add(reservation);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Réservation ajoutée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erreur lors de l'ajout : " + ex.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Erreur de validation : " + string.Join(", ", errors);
            }

            ViewBag.Salles = new SelectList(await _context.Salles.ToListAsync(), "IdSalle", "NomSalle");
            ViewBag.Utilisateurs = new SelectList(await _context.Utilisateurs.ToListAsync(), "IdUtilisateur", "Email");
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "IdClub", "NomClub");
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
            ViewBag.Salles = new SelectList(await _context.Salles.ToListAsync(), "IdSalle", "NomSalle", reservation.IdSalle);
            ViewBag.Utilisateurs = new SelectList(await _context.Utilisateurs.ToListAsync(), "IdUtilisateur", "Email", reservation.IdUtilisateur);
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "IdClub", "NomClub", reservation.IdClub);
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
                    if (!ReservationExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erreur lors de la modification : " + ex.Message;
                }
            }

            ViewBag.Salles = new SelectList(await _context.Salles.ToListAsync(), "IdSalle", "NomSalle", reservation.IdSalle);
            ViewBag.Utilisateurs = new SelectList(await _context.Utilisateurs.ToListAsync(), "IdUtilisateur", "Email", reservation.IdUtilisateur);
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "IdClub", "NomClub", reservation.IdClub);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Valider(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation != null)
            {
                reservation.Statut = "validee";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation validée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Annuler/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Annuler(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);
            if (reservation != null)
            {
                reservation.Statut = "annulee";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Réservation annulée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.ReservationsSalles.Any(e => e.IdReservation == id);
        }
    }
}