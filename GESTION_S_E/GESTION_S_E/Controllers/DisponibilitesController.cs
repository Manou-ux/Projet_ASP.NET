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
            var enseignants = await _context.Enseignants
                .Select(e => new { e.IdEnseignant, Nom = e.NomEnseignant + " " + e.PrenomEnseignant + " - " + e.Specialite })
                .ToListAsync();
            
            ViewBag.Enseignants = new SelectList(enseignants, "IdEnseignant", "Nom");
            return View();
        }

        // POST: Disponibilites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisponibiliteEnseignant disponibilite)
        {
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
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Erreur de validation : " + string.Join(", ", errors);
            }

            var enseignants = await _context.Enseignants
                .Select(e => new { e.IdEnseignant, Nom = e.NomEnseignant + " " + e.PrenomEnseignant + " - " + e.Specialite })
                .ToListAsync();
            ViewBag.Enseignants = new SelectList(enseignants, "IdEnseignant", "Nom");
            return View(disponibilite);
        }

        // GET: Disponibilites/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .FirstOrDefaultAsync(d => d.IdDispo == id);
            if (disponibilite == null)
            {
                return NotFound();
            }
            
            var enseignants = await _context.Enseignants
                .Select(e => new { e.IdEnseignant, Nom = e.NomEnseignant + " " + e.PrenomEnseignant + " - " + e.Specialite })
                .ToListAsync();
            ViewBag.Enseignants = new SelectList(enseignants, "IdEnseignant", "Nom", disponibilite.IdEnseignant);
            return View(disponibilite);
        }

        // POST: Disponibilites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DisponibiliteEnseignant disponibilite)
        {
            if (id != disponibilite.IdDispo)
            {
                return NotFound();
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
                    if (!DisponibiliteExists(id))
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

            var enseignants = await _context.Enseignants
                .Select(e => new { e.IdEnseignant, Nom = e.NomEnseignant + " " + e.PrenomEnseignant + " - " + e.Specialite })
                .ToListAsync();
            ViewBag.Enseignants = new SelectList(enseignants, "IdEnseignant", "Nom", disponibilite.IdEnseignant);
            return View(disponibilite);
        }

        // GET: Disponibilites/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .FirstOrDefaultAsync(d => d.IdDispo == id);
            if (disponibilite == null)
            {
                return NotFound();
            }
            return View(disponibilite);
        }

        // POST: Disponibilites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants.FindAsync(id);
            if (disponibilite != null)
            {
                _context.DisponibilitesEnseignants.Remove(disponibilite);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Disponibilité supprimée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Disponibilites/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants
                .Include(d => d.Enseignant)
                .FirstOrDefaultAsync(d => d.IdDispo == id);
            if (disponibilite == null)
            {
                return NotFound();
            }
            return View(disponibilite);
        }

        private bool DisponibiliteExists(int id)
        {
            return _context.DisponibilitesEnseignants.Any(e => e.IdDispo == id);
        }
    }
}