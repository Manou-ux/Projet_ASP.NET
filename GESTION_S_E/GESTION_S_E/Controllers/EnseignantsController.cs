using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class EnseignantsController : Controller
    {
        private readonly MonDbContext _context;

        public EnseignantsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Enseignants
        public async Task<IActionResult> Index()
        {
            var enseignants = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .ToListAsync();
            return View(enseignants);
        }

        // GET: Enseignants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enseignants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enseignant enseignant)
        {
            if (ModelState.IsValid)
            {
                // Rendre nullables les propriétés vides
                if (string.IsNullOrEmpty(enseignant.Specialite))
                    enseignant.Specialite = null;
                if (string.IsNullOrEmpty(enseignant.TelephoneEnseignant))
                    enseignant.TelephoneEnseignant = null;
                if (string.IsNullOrEmpty(enseignant.EmailPro))
                    enseignant.EmailPro = null;

                _context.Enseignants.Add(enseignant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enseignant ajouté avec succès !";
                return RedirectToAction(nameof(Index));
            }
            return View(enseignant);
        }

        // GET: Enseignants/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var enseignant = await _context.Enseignants.FindAsync(id);
            if (enseignant == null)
            {
                return NotFound();
            }
            return View(enseignant);
        }

        // POST: Enseignants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enseignant enseignant)
        {
            if (id != enseignant.IdEnseignant)
            {
                return NotFound();
            }

            var existingEnseignant = await _context.Enseignants.FindAsync(id);
            if (existingEnseignant == null)
            {
                return NotFound();
            }

            // Copier explicitement les valeurs reçues depuis le formulaire
            existingEnseignant.NomEnseignant = enseignant.NomEnseignant?.Trim() ?? "";
            existingEnseignant.PrenomEnseignant = enseignant.PrenomEnseignant?.Trim() ?? "";
            existingEnseignant.Specialite = enseignant.Specialite?.Trim() ?? "";
            // La colonne 'telephone_enseignant' est NOT NULL en base : éviter d'écrire null
            existingEnseignant.TelephoneEnseignant = enseignant.TelephoneEnseignant?.Trim() ?? "";
            existingEnseignant.EmailPro = enseignant.EmailPro?.Trim() ?? "";
            existingEnseignant.IdUtilisateur = enseignant.IdUtilisateur;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enseignant modifié avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Enseignants.Any(e => e.IdEnseignant == id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // GET: Enseignants/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var enseignant = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(e => e.IdEnseignant == id);
            if (enseignant == null)
            {
                return NotFound();
            }
            return View(enseignant);
        }

        // POST: Enseignants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enseignant = await _context.Enseignants.FindAsync(id);
            if (enseignant != null)
            {
                _context.Enseignants.Remove(enseignant);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enseignant supprimé avec succès !";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Enseignants/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var enseignant = await _context.Enseignants
                .Include(e => e.Utilisateur)
                .FirstOrDefaultAsync(e => e.IdEnseignant == id);
            if (enseignant == null)
            {
                return NotFound();
            }
            return View(enseignant);
        }
    }
}