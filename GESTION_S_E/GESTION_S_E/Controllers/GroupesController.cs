// GESTION_S_E\GESTION_S_E\Controllers\GroupesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class GroupesController : Controller
    {
        private readonly MonDbContext _context;

        public GroupesController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Groupes
        public async Task<IActionResult> Index()
        {
            var groupes = await _context.Groupes
                .Include(g => g.Classe)
                .ToListAsync();
            return View(groupes);
        }

        // GET: Groupes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Classes = await _context.Classes.ToListAsync();
            return View();
        }

        // POST: Groupes/Create (SANS ValidateAntiForgeryToken pour test)
        [HttpPost]
        public async Task<IActionResult> Create(string NomGroupe, int IdClasse)
        {
            // Log pour debug
            Console.WriteLine($"NomGroupe: {NomGroupe}");
            Console.WriteLine($"IdClasse: {IdClasse}");
            
            if (string.IsNullOrWhiteSpace(NomGroupe))
            {
                ModelState.AddModelError("NomGroupe", "Le nom du groupe est requis");
                ViewBag.Classes = await _context.Classes.ToListAsync();
                return View();
            }
            
            if (IdClasse <= 0)
            {
                ModelState.AddModelError("IdClasse", "Veuillez sélectionner une classe");
                ViewBag.Classes = await _context.Classes.ToListAsync();
                return View();
            }
            
            var groupe = new Groupe
            {
                NomGroupe = NomGroupe,
                IdClasse = IdClasse
            };
            
            _context.Groupes.Add(groupe);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Groupe ajouté avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: Groupes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var groupe = await _context.Groupes.FindAsync(id);
            if (groupe == null)
            {
                return NotFound();
            }
            ViewBag.Classes = await _context.Classes.ToListAsync();
            return View(groupe);
        }

        // POST: Groupes/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string NomGroupe, int IdClasse)
        {
            var groupe = await _context.Groupes.FindAsync(id);
            if (groupe == null)
            {
                return NotFound();
            }
            
            if (string.IsNullOrWhiteSpace(NomGroupe))
            {
                ModelState.AddModelError("NomGroupe", "Le nom du groupe est requis");
                ViewBag.Classes = await _context.Classes.ToListAsync();
                return View(groupe);
            }
            
            if (IdClasse <= 0)
            {
                ModelState.AddModelError("IdClasse", "Veuillez sélectionner une classe");
                ViewBag.Classes = await _context.Classes.ToListAsync();
                return View(groupe);
            }
            
            groupe.NomGroupe = NomGroupe;
            groupe.IdClasse = IdClasse;
            
            _context.Update(groupe);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Groupe modifié avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: Groupes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var groupe = await _context.Groupes
                .Include(g => g.Classe)
                .FirstOrDefaultAsync(g => g.IdGroupe == id);
            if (groupe == null)
            {
                return NotFound();
            }
            return View(groupe);
        }

        // POST: Groupes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupe = await _context.Groupes.FindAsync(id);
            if (groupe != null)
            {
                _context.Groupes.Remove(groupe);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Groupe supprimé avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Groupes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var groupe = await _context.Groupes
                .Include(g => g.Classe)
                .FirstOrDefaultAsync(g => g.IdGroupe == id);
            if (groupe == null)
            {
                return NotFound();
            }
            return View(groupe);
        }
    }
}