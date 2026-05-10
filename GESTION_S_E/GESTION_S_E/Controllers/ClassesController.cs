using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    // [Authorize(Roles = "admin,scolarite")]  ← COMMENTÉ
    public class ClassesController : Controller
    {
        private readonly MonDbContext _context;

        public ClassesController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes.ToListAsync();
            return View(classes);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Classe classe)
        {
            if (ModelState.IsValid)
            {
                _context.Classes.Add(classe);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Classe ajoutée avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(classe);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var classe = await _context.Classes.FindAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            return View(classe);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Classe classe)
        {
            if (id != classe.IdClasse)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classe);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Classe modifiée avec succès !";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Classes.Any(e => e.IdClasse == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(classe);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var classe = await _context.Classes.FindAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            return View(classe);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classe = await _context.Classes.FindAsync(id);
            if (classe != null)
            {
                _context.Classes.Remove(classe);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Classe supprimée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}