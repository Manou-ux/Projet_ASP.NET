using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class ScolaritesController : Controller
    {
        private readonly MonDbContext _context;

        public ScolaritesController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Scolarites
        public async Task<IActionResult> Index()
        {
            var scolarites = await _context.Scolarites
                .Include(s => s.Utilisateur)
                .ToListAsync();
            return View(scolarites);
        }

        // GET: Scolarites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scolarites/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Scolarite scolarite)
        {
            if (ModelState.IsValid)
            {
                // Rendre nullables les propriétés vides
                if (string.IsNullOrEmpty(scolarite.Fonction))
                    scolarite.Fonction = null;
                if (string.IsNullOrEmpty(scolarite.Telephone))
                    scolarite.Telephone = null;
                if (string.IsNullOrEmpty(scolarite.Bureau))
                    scolarite.Bureau = null;

                _context.Scolarites.Add(scolarite);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Agent de scolarité ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(scolarite);
        }

        // GET: Scolarites/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var scolarite = await _context.Scolarites.FindAsync(id);
            if (scolarite == null)
            {
                return NotFound();
            }
            return View(scolarite);
        }

        // POST: Scolarites/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Scolarite scolarite)
        {
            if (id != scolarite.IdScolarite)
            {
                return NotFound();
            }

            ModelState.Remove("Utilisateur");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scolarite);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Agent de scolarité modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Scolarites.Any(s => s.IdScolarite == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }
            return View(scolarite);
        }

        // GET: Scolarites/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var scolarite = await _context.Scolarites
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(s => s.IdScolarite == id);
            if (scolarite == null)
            {
                return NotFound();
            }
            return View(scolarite);
        }

        // POST: Scolarites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scolarite = await _context.Scolarites.FindAsync(id);
            if (scolarite != null)
            {
                _context.Scolarites.Remove(scolarite);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Agent de scolarité supprimé avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Scolarites/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var scolarite = await _context.Scolarites
                .Include(s => s.Utilisateur)
                .FirstOrDefaultAsync(s => s.IdScolarite == id);
            if (scolarite == null)
            {
                return NotFound();
            }
            return View(scolarite);
        }
    }
}