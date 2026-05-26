using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            ViewBag.IdSalle = new SelectList(_context.Salles, "IdSalle", "NomSalle", edt?.IdSalle);
            var enseignantsQuery = _context.Enseignants.Select(e => new {
                IdEnseignant = e.IdEnseignant,
                NomComplet = e.NomEnseignant + " " + e.PrenomEnseignant
            });
            ViewBag.IdEnseignant = new SelectList(enseignantsQuery, "IdEnseignant", "NomComplet", edt?.IdEnseignant);
            ViewBag.IdMatiere = new SelectList(_context.Matieres, "IdMatiere", "NomMatiere", edt?.IdMatiere);
            ViewBag.IdClasse = new SelectList(_context.Classes, "IdClasse", "NomClasse", edt?.IdClasse);
            ViewBag.IdGroupe = new SelectList(_context.Groupes, "IdGroupe", "NomGroupe", edt?.IdGroupe);
        }

        // =======================================================
        // ViewModel pour l'affichage hebdomadaire (imbriqué)
        // =======================================================
        public class WeeklyTimetableViewModel
        {
            public DateTime Lundi { get; set; }
            public int ClasseId { get; set; }
            public List<Classe> Classes { get; set; } // Ajout pour contourner le problème du SelectList
            public Dictionary<DayOfWeek, Dictionary<TimeSpan, List<EmploiDuTemps>>> CoursParJourEtHeure { get; set; }
        }

        // =======================================================
        // 9. WEEKLY VIEW : Affichage planning hebdomadaire (version finale)
        // =======================================================
        public async Task<IActionResult> WeeklyView(int? classeId, DateTime? dateDebut)
        {
            // 1. Récupérer TOUTES les classes
            var toutesLesClasses = await _context.Classes.ToListAsync();
            if (!toutesLesClasses.Any())
            {
                TempData["Error"] = "Aucune classe trouvée. Veuillez d'abord créer une classe via /Classes/Create.";
                var emptyModel = new WeeklyTimetableViewModel
                {
                    Lundi = GetStartOfWeek(DateTime.Today),
                    Classes = new List<Classe>(),
                    CoursParJourEtHeure = new Dictionary<DayOfWeek, Dictionary<TimeSpan, List<EmploiDuTemps>>>()
                };
                return View(emptyModel);
            }

            // 2. Sélectionner la classe par défaut (première) si aucun ID n'est passé
            if (classeId == null)
                classeId = toutesLesClasses.First().IdClasse;

            // 3. Calcul de la semaine
            DateTime today = dateDebut ?? DateTime.Today;
            DateTime lundi = GetStartOfWeek(today);
            DateTime dimanche = lundi.AddDays(6);

            // 4. Récupérer les cours pour cette classe et cette semaine
            var cours = await _context.EmploisDuTemps
                .Include(e => e.Matiere)
                .Include(e => e.Enseignant)
                .Include(e => e.Salle)
                .Include(e => e.Classe)
                .Where(e => e.IdClasse == classeId && e.DateCours >= lundi && e.DateCours <= dimanche)
                .ToListAsync();

            if (!cours.Any())
                TempData["Info"] = $"Aucun cours programmé pour la classe sélectionnée du {lundi:dd/MM/yyyy} au {dimanche:dd/MM/yyyy}.";

            // 5. Remplir le ViewBag (optionnel, gardé pour compatibilité)
            ViewBag.DateDebut = lundi.ToString("yyyy-MM-dd");

            // 6. Construire le modèle
            var model = new WeeklyTimetableViewModel
            {
                Lundi = lundi,
                ClasseId = classeId.Value,
                Classes = toutesLesClasses, // Passage de la liste complète
                CoursParJourEtHeure = OrganiserCoursParJourEtHeure(cours, lundi)
            };
            return View(model);
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }

        private Dictionary<DayOfWeek, Dictionary<TimeSpan, List<EmploiDuTemps>>> OrganiserCoursParJourEtHeure(List<EmploiDuTemps> cours, DateTime lundi)
        {
            var result = new Dictionary<DayOfWeek, Dictionary<TimeSpan, List<EmploiDuTemps>>>();

            for (int i = 0; i < 5; i++)
            {
                DayOfWeek jour = (DayOfWeek)((int)DayOfWeek.Monday + i);
                result[jour] = new Dictionary<TimeSpan, List<EmploiDuTemps>>();
            }

            var creneaux = new List<TimeSpan>();
            for (int h = 7; h <= 18; h++)
                creneaux.Add(TimeSpan.FromHours(h));

            foreach (var jour in result.Keys.ToList())
            {
                foreach (var creneau in creneaux)
                {
                    result[jour][creneau] = new List<EmploiDuTemps>();
                }
            }

            foreach (var c in cours)
            {
                DayOfWeek jourCours = c.DateCours.DayOfWeek;
                if (result.ContainsKey(jourCours))
                {
                    TimeSpan debut = c.HeureDebut;
                    TimeSpan cle = new TimeSpan(debut.Hours, 0, 0);
                    if (result[jourCours].ContainsKey(cle))
                        result[jourCours][cle].Add(c);
                    else
                        result[jourCours][cle] = new List<EmploiDuTemps> { c };
                }
            }
            return result;
        }
    }
}