using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class ElevesController : Controller
    {
        private readonly MonDbContext _context;

        public ElevesController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Eleves
        public async Task<IActionResult> Index()
        {
            var eleves = await _context.Eleves
                .Include(e => e.Classe)
                .ToListAsync();
            return View(eleves);
        }

        // GET: Eleves/Create
        public IActionResult Create()
        {
            ViewBag.Classes = new SelectList(_context.Classes, "IdClasse", "NomClasse");
            return View();
        }

        // POST: Eleves/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Eleve eleve)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(eleve.Matricule))
                {
                    eleve.Matricule = $"ELEV{DateTime.Now.Ticks}";
                }

                _context.Eleves.Add(eleve);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Élève ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Classes = new SelectList(_context.Classes, "IdClasse", "NomClasse", eleve.IdClasse);
            return View(eleve);
        }

        // GET: Eleves/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var eleve = await _context.Eleves
                .FirstOrDefaultAsync(e => e.IdEleve == id);

            if (eleve == null)
            {
                return NotFound();
            }
            ViewBag.Classes = new SelectList(_context.Classes, "IdClasse", "NomClasse", eleve.IdClasse);

            return View(eleve);
        }

        // POST: Eleves/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Eleve eleve)
        {
            if (id != eleve.IdEleve)
            {
                return NotFound();
            }

            // Si une date de naissance est fournie, la convertir en UTC
            if (eleve.DateNaissance.HasValue)
            {
                eleve.DateNaissance = DateTime.SpecifyKind(eleve.DateNaissance.Value, DateTimeKind.Utc);
            }

            // Supprimer les erreurs de validation pour les propriétés de navigation
            ModelState.Remove("Classe");
            ModelState.Remove("Utilisateur");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eleve);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Élève modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur: {ex.Message}");
                }
            }
            ViewBag.Classes = new SelectList(_context.Classes, "IdClasse", "NomClasse", eleve.IdClasse);
            return View(eleve);
        }
        // GET: Eleves/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var eleve = await _context.Eleves
                .Include(e => e.Classe)
                .FirstOrDefaultAsync(e => e.IdEleve == id);
            if (eleve == null)
            {
                return NotFound();
            }
            return View(eleve);
        }

        // POST: Eleves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eleve = await _context.Eleves.FindAsync(id);
            if (eleve != null)
            {
                _context.Eleves.Remove(eleve);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Élève supprimé avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Eleves/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var eleve = await _context.Eleves
                .Include(e => e.Classe)
                .FirstOrDefaultAsync(e => e.IdEleve == id);
            if (eleve == null)
            {
                return NotFound();
            }
            return View(eleve);
        }
    }
}