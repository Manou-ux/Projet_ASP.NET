using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GESTION_S_E.Controllers
{
    public class EmploiDuTempsController : Controller
    {
        private readonly MonDbContext _context;

        public EmploiDuTempsController(MonDbContext context)
        {
            _context = context;
        }

        // =======================================================
        // 1. INDEX : Liste complète des emplois du temps
        // =======================================================
        public async Task<IActionResult> Index()
        {
            var emplois = _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Classe)
                .Include(e => e.Groupe);

            return View(await emplois.ToListAsync());
        }

        // =======================================================
        // 2. DETAILS : Voir un élément d'EDT spécifique
        // =======================================================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var emploiDuTemps = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Classe)
                .Include(e => e.Groupe)
                .FirstOrDefaultAsync(m => m.IdEmploi == id);

            if (emploiDuTemps == null) return NotFound();

            return View(emploiDuTemps);
        }

        // =======================================================
        // 3. CREATE (GET) : Formulaire de création
        // =======================================================
        public IActionResult Create()
        {
            PopulateDropDownListData();
            return View();
        }

        // =======================================================
        // 4. CREATE (POST) : Enregistrement de l'EDT
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmploi,DateCours,HeureDebut,HeureFin,Semestre,Statut,IdSalle,IdEnseignant,IdMatiere,IdClasse,IdGroupe")] EmploiDuTemps emploiDuTemps)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emploiDuTemps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // DEBUG : Si ça ne marche toujours pas, regarde ton terminal, 
            // les erreurs exactes s'afficheront ici :
            foreach (var modelStateKey in ModelState.Keys)
            {
                var value = ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    Console.WriteLine($"Erreur sur la clé [{modelStateKey}] : {error.ErrorMessage}");
                }
            }

            PopulateDropDownListData(emploiDuTemps);
            return View(emploiDuTemps);
        }

        // =======================================================
        // 5. EDIT (GET) : Formulaire de modification
        // =======================================================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var emploiDuTemps = await _context.EmploisDuTemps.FindAsync(id);
            if (emploiDuTemps == null) return NotFound();

            PopulateDropDownListData(emploiDuTemps);
            return View(emploiDuTemps);
        }

        // =======================================================
        // 6. EDIT (POST) : Enregistrement des modifications
        // =======================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmploi,DateCours,HeureDebut,HeureFin,Semestre,Statut,IdSalle,IdEnseignant,IdMatiere,IdClasse,IdGroupe")] EmploiDuTemps emploiDuTemps)
        {
            if (id != emploiDuTemps.IdEmploi) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emploiDuTemps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmploiDuTempsExists(emploiDuTemps.IdEmploi)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // DEBUG : Permet de voir dans le terminal s'il y a un champ invalide
            foreach (var modelStateKey in ModelState.Keys)
            {
                var value = ModelState[modelStateKey];
                foreach (var error in value.Errors)
                {
                    Console.WriteLine($"[EDIT] Erreur sur la clé [{modelStateKey}] : {error.ErrorMessage}");
                }
            }

            PopulateDropDownListData(emploiDuTemps);
            return View(emploiDuTemps);
        }

        // =======================================================
        // 7. DELETE (GET) : Page de confirmation
        // =======================================================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var emploiDuTemps = await _context.EmploisDuTemps
                .Include(e => e.Salle)
                .Include(e => e.Enseignant)
                .Include(e => e.Matiere)
                .Include(e => e.Classe)
                .Include(e => e.Groupe)
                .FirstOrDefaultAsync(m => m.IdEmploi == id);

            if (emploiDuTemps == null) return NotFound();

            return View(emploiDuTemps);
        }

        // =======================================================
        // 8. DELETE (POST) : Suppression définitive
        // =======================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emploiDuTemps = await _context.EmploisDuTemps.FindAsync(id);
            if (emploiDuTemps != null)
            {
                _context.EmploisDuTemps.Remove(emploiDuTemps);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmploiDuTempsExists(int id)
        {
            return _context.EmploisDuTemps.Any(e => e.IdEmploi == id);
        }

        // =======================================================
        // CENTRALISATION ET MAP DES PROPRIÉTÉS DES MODÈLES SÉLECTIONNÉS
        // =======================================================
        private void PopulateDropDownListData(EmploiDuTemps edt = null)
        {
            // Salles : Value = IdSalle, Text = NomSalle
            ViewBag.IdSalle = new SelectList(_context.Salles, "IdSalle", "NomSalle", edt?.IdSalle);

            // Enseignants : Value = IdEnseignant, Text = "NomEnseignant PrenomEnseignant"
            var enseignantsQuery = _context.Enseignants.Select(e => new {
                IdEnseignant = e.IdEnseignant,
                NomComplet = e.NomEnseignant + " " + e.PrenomEnseignant
            });
            ViewBag.IdEnseignant = new SelectList(enseignantsQuery, "IdEnseignant", "NomComplet", edt?.IdEnseignant);

            // Matières : Value = IdMatiere, Text = NomMatiere
            ViewBag.IdMatiere = new SelectList(_context.Matieres, "IdMatiere", "NomMatiere", edt?.IdMatiere);

            // Classes : Value = IdClasse, Text = NomClasse
            ViewBag.IdClasse = new SelectList(_context.Classes, "IdClasse", "NomClasse", edt?.IdClasse);

            // Groupes : Value = IdGroupe, Text = NomGroupe
            ViewBag.IdGroupe = new SelectList(_context.Groupes, "IdGroupe", "NomGroupe", edt?.IdGroupe);
        }
    }
}