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
            var reservations = await _context.ReservationsSalles
                .Include(r => r.Salle)
                .Include(r => r.Utilisateur)
                .Include(r => r.Club)
                .OrderBy(r => r.DateReservation)
                .ToListAsync();

            return View(reservations);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();
            return View();
        }

        // POST: Reservations/Create
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

            if (ModelState.IsValid)
            {
                try
                {
                    reservation.Statut = "en_attente";
                    reservation.DateReservation = DateTime.UtcNow;   // ou DateTime.Now selon ton besoin

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

            await LoadSelectLists();
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.ReservationsSalles
                .FindAsync(id);

            if (reservation == null) return NotFound();

            await LoadSelectLists(reservation.IdSalle, reservation.IdUtilisateur, reservation.IdClub);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationSalle reservation)
        {
            ModelState.Remove("Salle");
            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");
            if (id != reservation.IdReservation) return NotFound();

            if (reservation.HeureFin <= reservation.HeureDebut)
            {
                ModelState.AddModelError("HeureFin", "L'heure de fin doit être supérieure à l'heure de début.");
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
                    if (!ReservationExists(id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erreur lors de la modification : " + ex.Message;
                }
            }

            await LoadSelectLists(reservation.IdSalle, reservation.IdUtilisateur, reservation.IdClub);
            return View(reservation);
        }

        private async Task LoadSelectLists(int? selectedSalle = null, int? selectedUtilisateur = null, int? selectedClub = null)
        {
            ViewBag.Salles = new SelectList(await _context.Salles.ToListAsync(), "IdSalle", "NomSalle", selectedSalle);
            ViewBag.Utilisateurs = new SelectList(await _context.Utilisateurs.ToListAsync(), "IdUtilisateur", "Email", selectedUtilisateur);
            ViewBag.Clubs = new SelectList(await _context.Clubs.ToListAsync(), "IdClub", "NomClub", selectedClub);
        }

        // Actions existantes (Delete, Valider, Annuler, Details...) restent fonctionnelles
        // Je te les garde telles quelles sauf si tu veux des modifications.

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

        private bool ReservationExists(int id)
        {
            return _context.ReservationsSalles.Any(e => e.IdReservation == id);
        }
    }
}