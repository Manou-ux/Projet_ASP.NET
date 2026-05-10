using Microsoft.AspNetCore.Mvc;
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
            ViewBag.Enseignants = await _context.Enseignants.ToListAsync();
            return View();
        }

        // POST: Disponibilites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisponibiliteEnseignant disponibilite)
        {
            if (string.IsNullOrEmpty(disponibilite.Jour))
                disponibilite.Jour = null;
            if (string.IsNullOrEmpty(disponibilite.TypeDispo))
                disponibilite.TypeDispo = null;

            if (ModelState.IsValid)
            {
                _context.DisponibilitesEnseignants.Add(disponibilite);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Disponibilité ajoutée avec succès !";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Enseignants = await _context.Enseignants.ToListAsync();
            return View(disponibilite);
        }

        // GET: Disponibilites/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var disponibilite = await _context.DisponibilitesEnseignants.FindAsync(id);
            if (disponibilite == null)
            {
                return NotFound();
            }
            ViewBag.Enseignants = await _context.Enseignants.ToListAsync();
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

            ModelState.Remove("Enseignant");

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
                    if (!_context.DisponibilitesEnseignants.Any(d => d.IdDispo == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            ViewBag.Enseignants = await _context.Enseignants.ToListAsync();
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
    }
}