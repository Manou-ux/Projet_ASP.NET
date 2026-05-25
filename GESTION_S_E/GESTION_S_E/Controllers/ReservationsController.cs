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

        public async Task<IActionResult> Index()
        {
            var reservations = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                    .ThenInclude(u => u.Enseignant)
                .Include(r => r.Club)
                .OrderByDescending(r => r.DateReservation)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> Calendar()
        {
            var reservations = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                    .ThenInclude(u => u.Enseignant)
                .Include(r => r.Club)
                .OrderBy(r => r.DateReservation)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                    .ThenInclude(u => u.Enseignant)
                .Include(r => r.Club)
                .FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
            {
                TempData["Error"] = "Réservation introuvable";
                return RedirectToAction(nameof(Index));
            }

            return View(reservation);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationSalle reservation)
        {
            ModelState.Remove("Salle");
            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");

            if (reservation.HeureFin <= reservation.HeureDebut)
            {
                ModelState.AddModelError("HeureFin", "L'heure de fin doit être supérieure à l'heure de début.");
            }

            // Validation : La date ne doit pas être dans le passé
            if (reservation.DateReservation.Date < DateTime.Today)
            {
                ModelState.AddModelError("DateReservation", "La date de réservation ne peut pas être dans le passé.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reservation.Statut = "en_attente";
                    // SUPPRIMÉ : reservation.DateReservation = DateTime.UtcNow;
                    // La date garde celle que l'utilisateur a choisie

                    _context.ReservationsSalles.Add(reservation);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Réservation ajoutée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
                }
            }

            await LoadSelectLists();
            return View(reservation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.ReservationsSalles.FindAsync(id);

            if (reservation == null)
                return NotFound();

            await LoadSelectLists(reservation.IdSalle, reservation.IdUtilisateur, reservation.IdClub);

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationSalle reservation)
        {
            ModelState.Remove("Salle");
            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");

            if (id != reservation.IdReservation)
                return NotFound();

            if (reservation.HeureFin <= reservation.HeureDebut)
            {
                ModelState.AddModelError("HeureFin", "L'heure de fin doit être supérieure à l'heure de début.");
            }

            // Validation : La date ne doit pas être dans le passé
            if (reservation.DateReservation.Date < DateTime.Today)
            {
                ModelState.AddModelError("DateReservation", "La date de réservation ne peut pas être dans le passé.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var reservationExistante = await _context.ReservationsSalles.FindAsync(id);

                    if (reservationExistante == null)
                        return NotFound();

                    reservationExistante.IdSalle = reservation.IdSalle;
                    reservationExistante.IdUtilisateur = reservation.IdUtilisateur;
                    reservationExistante.IdClub = reservation.IdClub;
                    reservationExistante.DateReservation = reservation.DateReservation; // AJOUT : Mise à jour de la date
                    reservationExistante.HeureDebut = reservation.HeureDebut;
                    reservationExistante.HeureFin = reservation.HeureFin;
                    reservationExistante.Motif = reservation.Motif;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Réservation modifiée avec succès !";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
                }
            }

            await LoadSelectLists(reservation.IdSalle, reservation.IdUtilisateur, reservation.IdClub);

            return View(reservation);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                    .ThenInclude(u => u.Enseignant)
                .Include(r => r.Club)
                .FirstOrDefaultAsync(r => r.IdReservation == id);

            if (reservation == null)
            {
                TempData["Error"] = "Réservation introuvable";
                return RedirectToAction(nameof(Index));
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var reservation = await _context.ReservationsSalles.FindAsync(id);

                if (reservation != null)
                {
                    _context.ReservationsSalles.Remove(reservation);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Réservation supprimée avec succès !";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.InnerException?.Message ?? ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

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

        private async Task LoadSelectLists(int? selectedSalle = null, int? selectedUtilisateur = null, int? selectedClub = null)
        {
            ViewBag.Salles = new SelectList(
                await _context.Salles.ToListAsync(),
                "IdSalle",
                "NomSalle",
                selectedSalle
            );

            var utilisateurs = await _context.Utilisateurs
                .Include(u => u.Enseignant)
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = u.Enseignant != null 
                        ? $"{u.Enseignant.PrenomEnseignant} {u.Enseignant.NomEnseignant}"
                        : u.Email
                })
                .ToListAsync();

            ViewBag.Utilisateurs = new SelectList(
                utilisateurs,
                "IdUtilisateur",
                "NomComplet",
                selectedUtilisateur
            );

            ViewBag.Clubs = new SelectList(
                await _context.Clubs.ToListAsync(),
                "IdClub",
                "NomClub",
                selectedClub
            );
        }

        private bool ReservationExists(int id)
        {
            return _context.ReservationsSalles.Any(e => e.IdReservation == id);
        }
    }
}