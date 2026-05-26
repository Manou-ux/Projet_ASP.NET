using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class DisponibilitesController : Controller
    {
        private readonly MonDbContext _context;

        public DisponibilitesController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Disponibilites
        public async Task<IActionResult> Index()
        {
            var disponibilites = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .OrderBy(d => d.Jour)
                .ThenBy(d => d.HeureDebut)
                .ToListAsync();

            return View(disponibilites);
        }

        // GET: Disponibilites/Create
        public async Task<IActionResult> Create()
        {
            await LoadEnseignantsSelectList();
            return View();
        }

        // POST: Disponibilites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisponibiliteEnseignant disponibilite)
        {
            ModelState.Remove("Enseignant");
            if (disponibilite.HeureFin <= disponibilite.HeureDebut)
            {
                ModelState.AddModelError("HeureFin", "L'heure de fin doit être supérieure à l'heure de début.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.DisponibilitesEnseignants.Add(disponibilite);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Disponibilité ajoutée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erreur lors de l'ajout : " + ex.Message;
                }
            }

            // Recharger le SelectList en cas d'erreur
            await LoadEnseignantsSelectList();
            return View(disponibilite);
        }

        // GET: Disponibilites/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .FirstOrDefaultAsync(d => d.IdDispo == id);

            if (disponibilite == null) return NotFound();

            await LoadEnseignantsSelectList(disponibilite.IdEnseignant);
            return View(disponibilite);
        }



        // GET: Disponibilites/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // On récupère la disponibilité en incluant les données de l'enseignant lié
            var disponibilite = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .FirstOrDefaultAsync(d => d.IdDispo == id);

            // Si aucune disponibilité ne correspond à l'ID, on renvoie une erreur 404
            if (disponibilite == null)
            {
                return NotFound();
            }

            // On renvoie la vue avec le modèle trouvé
            return View(disponibilite);
        }


        // POST: Disponibilites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DisponibiliteEnseignant disponibilite)
        {
            ModelState.Remove("Enseignant");
            if (id != disponibilite.IdDispo) return NotFound();

            if (disponibilite.HeureFin <= disponibilite.HeureDebut)
            {
                ModelState.AddModelError("HeureFin", "L'heure de fin doit être supérieure à l'heure de début.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disponibilite);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Disponibilité modifiée avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisponibiliteExists(id)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Erreur lors de la modification : " + ex.Message;
                }
            }

            await LoadEnseignantsSelectList(disponibilite.IdEnseignant);
            return View(disponibilite);
        }

        private async Task LoadEnseignantsSelectList(int? selectedId = null)
        {
            var enseignants = await _context.Enseignants
                .Select(e => new 
                { 
                    e.IdEnseignant, 
                    NomComplet = e.NomEnseignant + " " + e.PrenomEnseignant + " - " + (e.Specialite ?? "") 
                })
                .ToListAsync();

            ViewBag.Enseignants = new SelectList(enseignants, "IdEnseignant", "NomComplet", selectedId);
        }

        // Les autres actions (Delete, Details...) restent presque identiques
        // Je te les garde si tu veux, mais pour l'instant je me concentre sur le Create/Edit qui posaient problème.

        private bool DisponibiliteExists(int id)
        {
            return _context.DisponibilitesEnseignants.Any(e => e.IdDispo == id);
        }
    }
}