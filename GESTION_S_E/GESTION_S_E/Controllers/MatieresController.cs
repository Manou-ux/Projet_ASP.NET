// GESTION_S_E\GESTION_S_E\Controllers\MatieresController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class MatieresController : Controller
    {
        private readonly MonDbContext _context;

        public MatieresController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Matieres
        public async Task<IActionResult> Index()
        {
            var matieres = await _context.Matieres.ToListAsync();
            return View(matieres);
        }

        // GET: Matieres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Matieres/Create
        [HttpPost]
        public async Task<IActionResult> Create(string NomMatiere, string CodeMatiere, int? VolumeHoraire, decimal Coefficient)
        {
            // Log pour debug
            Console.WriteLine($"NomMatiere: {NomMatiere}");
            Console.WriteLine($"CodeMatiere: {CodeMatiere}");
            Console.WriteLine($"VolumeHoraire: {VolumeHoraire}");
            Console.WriteLine($"Coefficient: {Coefficient}");

            if (string.IsNullOrWhiteSpace(NomMatiere))
            {
                TempData["Error"] = "Le nom de la matière est requis";
                return RedirectToAction(nameof(Index));
            }

            if (Coefficient <= 0)
            {
                TempData["Error"] = "Le coefficient doit être supérieur à 0";
                return RedirectToAction(nameof(Index));
            }

            var matiere = new Matiere
            {
                NomMatiere = NomMatiere,
                CodeMatiere = CodeMatiere ?? string.Empty,
                VolumeHoraire = VolumeHoraire,
                Coefficient = Coefficient
            };

            _context.Matieres.Add(matiere);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Matière '{matiere.NomMatiere}' ajoutée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: Matieres/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                return NotFound();
            }
            return View(matiere);
        }

        // POST: Matieres/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string NomMatiere, string CodeMatiere, int? VolumeHoraire, decimal Coefficient)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                TempData["Error"] = "Matière non trouvée !";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrWhiteSpace(NomMatiere))
            {
                TempData["Error"] = "Le nom de la matière est requis";
                return RedirectToAction(nameof(Index));
            }

            if (Coefficient <= 0)
            {
                TempData["Error"] = "Le coefficient doit être supérieur à 0";
                return RedirectToAction(nameof(Index));
            }

            matiere.NomMatiere = NomMatiere;
            matiere.CodeMatiere = CodeMatiere ?? string.Empty;
            matiere.VolumeHoraire = VolumeHoraire;
            matiere.Coefficient = Coefficient;

            _context.Update(matiere);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Matière '{matiere.NomMatiere}' modifiée avec succès !";
            return RedirectToAction(nameof(Index));
        }

        // GET: Matieres/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere == null)
            {
                return NotFound();
            }
            return View(matiere);
        }

        // POST: Matieres/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);
            if (matiere != null)
            {
                string nomMatiere = matiere.NomMatiere;
                _context.Matieres.Remove(matiere);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Matière '{nomMatiere}' supprimée avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}