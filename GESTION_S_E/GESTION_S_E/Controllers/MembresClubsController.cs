using GESTION_S_E.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GESTION_S_E.Controllers
{
    public class MembresClubsController : Controller
    {
        private readonly MonDbContext _context;

        public MembresClubsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: MembresClubs
        public async Task<IActionResult> Index()
        {
            var membresClubs = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .Include(m => m.Utilisateur)
                .Include(m => m.Club)
                .OrderByDescending(m => m.DateAdhesion)
                .ToListAsync();
            return View(membresClubs);
        }

        // GET: MembresClubs/Details/5
        public async Task<IActionResult> Details(int? idUtilisateur, int? idClub)
        {
            if (idUtilisateur == null || idClub == null)
            {
                return NotFound();
            }

            var membreClub = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .Include(m => m.Utilisateur)
                .Include(m => m.Club)
                .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

            if (membreClub == null)
            {
                return NotFound();
            }

            return View(membreClub);
        }

        // GET: MembresClubs/Create
        public IActionResult Create()
        {
            ViewBag.IdUtilisateur = new SelectList(_context.Utilisateurs, "IdUtilisateur", "Email");
            ViewBag.IdClub = new SelectList(_context.Clubs, "IdClub", "NomClub");
            return View();
        }

        // POST: MembresClubs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtilisateur,IdClub,RoleMembre")] MembreClub membreClub)
        {
            // Retirer les propriétés de navigation de la validation
            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");

            // Vérifier si le membre existe déjà dans ce club
            var existeDeja = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .AnyAsync(m => m.IdUtilisateur == membreClub.IdUtilisateur && m.IdClub == membreClub.IdClub);

            if (existeDeja)
            {
                ModelState.AddModelError("", "Cet utilisateur est déjà membre de ce club.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Utiliser UTC pour PostgreSQL au lieu de DateTime.Now
                    membreClub.DateAdhesion = DateTime.UtcNow;

                    _context.MembreClubs.Add(membreClub);  // Changé: MembresClubs -> MembreClubs
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Le membre a été ajouté au club avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"Erreur technique : {innerException}");
                    System.Diagnostics.Debug.WriteLine($"Erreur complète: {ex.ToString()}");
                }
            }

            ViewBag.IdUtilisateur = new SelectList(_context.Utilisateurs, "IdUtilisateur", "Email", membreClub.IdUtilisateur);
            ViewBag.IdClub = new SelectList(_context.Clubs, "IdClub", "NomClub", membreClub.IdClub);
            return View(membreClub);
        }

        // GET: MembresClubs/Edit/5
        public async Task<IActionResult> Edit(int? idUtilisateur, int? idClub)
        {
            if (idUtilisateur == null || idClub == null)
            {
                return NotFound();
            }

            var membreClub = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

            if (membreClub == null)
            {
                return NotFound();
            }

            ViewBag.IdUtilisateur = new SelectList(_context.Utilisateurs, "IdUtilisateur", "Email", membreClub.IdUtilisateur);
            ViewBag.IdClub = new SelectList(_context.Clubs, "IdClub", "NomClub", membreClub.IdClub);
            return View(membreClub);
        }

        // POST: MembresClubs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idUtilisateur, int idClub, [Bind("IdUtilisateur,IdClub,RoleMembre,DateAdhesion")] MembreClub membreClub)
        {
            if (idUtilisateur != membreClub.IdUtilisateur || idClub != membreClub.IdClub)
            {
                return NotFound();
            }

            ModelState.Remove("Utilisateur");
            ModelState.Remove("Club");

            if (ModelState.IsValid)
            {
                try
                {
                    // Récupérer l'entité existante
                    var membreExistant = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                        .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

                    if (membreExistant == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour uniquement le rôle (la date d'adhésion ne change pas)
                    membreExistant.RoleMembre = membreClub.RoleMembre;

                    _context.Update(membreExistant);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Le rôle du membre a été modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de la modification : {ex.InnerException?.Message ?? ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Erreur modification: {ex.ToString()}");
                }
            }

            ViewBag.IdUtilisateur = new SelectList(_context.Utilisateurs, "IdUtilisateur", "Email", membreClub.IdUtilisateur);
            ViewBag.IdClub = new SelectList(_context.Clubs, "IdClub", "NomClub", membreClub.IdClub);
            return View(membreClub);
        }

        // GET: MembresClubs/Delete/5
        public async Task<IActionResult> Delete(int? idUtilisateur, int? idClub)
        {
            if (idUtilisateur == null || idClub == null)
            {
                return NotFound();
            }

            var membreClub = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .Include(m => m.Utilisateur)
                .Include(m => m.Club)
                .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

            if (membreClub == null)
            {
                return NotFound();
            }

            return View(membreClub);
        }

        // POST: MembresClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idUtilisateur, int idClub)
        {
            try
            {
                var membreClub = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                    .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

                if (membreClub != null)
                {
                    _context.MembreClubs.Remove(membreClub);  // Changé: MembresClubs -> MembreClubs
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Le membre a été retiré du club avec succès.";
                }
                else
                {
                    TempData["Error"] = "Membre non trouvé";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la suppression : {ex.InnerException?.Message ?? ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Erreur suppression: {ex.ToString()}");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: MembresClubs/ParClub/5
        public async Task<IActionResult> ParClub(int idClub)
        {
            var membres = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .Include(m => m.Utilisateur)
                .Where(m => m.IdClub == idClub)
                .OrderByDescending(m => m.DateAdhesion)
                .ToListAsync();

            var club = await _context.Clubs.FindAsync(idClub);
            ViewBag.ClubNom = club?.NomClub ?? "Club inconnu";

            return View(membres);
        }
        // GET: MembresClubs/GetMembresByClubJson
        [HttpGet]
        public async Task<IActionResult> GetMembresByClubJson(int clubId)
        {
            var membres = await _context.MembreClubs
                .Include(m => m.Utilisateur)
                .Where(m => m.IdClub == clubId)
                .Select(m => new
                {
                    m.IdUtilisateur,
                    m.IdClub,
                    m.RoleMembre,
                    m.DateAdhesion,
                    Email = m.Utilisateur != null ? m.Utilisateur.Email : "",
                    // Supprimer Nom et Prenom s'ils n'existent pas dans Utilisateur
                    // Utiliser uniquement Email ou d'autres propriétés existantes
                    NomUtilisateur = m.Utilisateur != null ? m.Utilisateur.Email : "", // ou utiliser une autre propriété
                    PrenomUtilisateur = "" // Laisser vide si n'existe pas
                })
                .OrderByDescending(m => m.DateAdhesion)
                .ToListAsync();

            return Json(membres);
        }

        // POST: MembresClubs/AjouterMembre
        [HttpPost]
        public async Task<IActionResult> AjouterMembre([FromBody] AjouterMembreDto model)
        {
            try
            {
                // Vérifier si le membre existe déjà
                var existe = await _context.MembreClubs
                    .AnyAsync(m => m.IdUtilisateur == model.IdUtilisateur && m.IdClub == model.IdClub);

                if (existe)
                {
                    return BadRequest("Ce membre est déjà dans le club");
                }

                var membre = new MembreClub
                {
                    IdUtilisateur = model.IdUtilisateur,
                    IdClub = model.IdClub,
                    RoleMembre = model.RoleMembre,
                    DateAdhesion = DateTime.UtcNow
                };

                _context.MembreClubs.Add(membre);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: MembresClubs/ModifierRole
        [HttpPost]
        public async Task<IActionResult> ModifierRole([FromBody] ModifierRoleDto model)
        {
            try
            {
                var membre = await _context.MembreClubs
                    .FirstOrDefaultAsync(m => m.IdUtilisateur == model.IdUtilisateur && m.IdClub == model.IdClub);

                if (membre == null)
                {
                    return NotFound("Membre non trouvé");
                }

                membre.RoleMembre = model.RoleMembre;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: MembresClubs/SupprimerMembre
        [HttpDelete]
        public async Task<IActionResult> SupprimerMembre(int idUtilisateur, int idClub)
        {
            try
            {
                var membre = await _context.MembreClubs
                    .FirstOrDefaultAsync(m => m.IdUtilisateur == idUtilisateur && m.IdClub == idClub);

                if (membre == null)
                {
                    return NotFound("Membre non trouvé");
                }

                _context.MembreClubs.Remove(membre);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DTOs pour les requêtes
        public class AjouterMembreDto
        {
            public int IdUtilisateur { get; set; }
            public int IdClub { get; set; }
            public string RoleMembre { get; set; }
        }

        public class ModifierRoleDto
        {
            public int IdUtilisateur { get; set; }
            public int IdClub { get; set; }
            public string RoleMembre { get; set; }
        }

        // GET: MembresClubs/ParUtilisateur/5
        public async Task<IActionResult> ParUtilisateur(int idUtilisateur)
        {
            var clubs = await _context.MembreClubs  // Changé: MembresClubs -> MembreClubs
                .Include(m => m.Club)
                .Where(m => m.IdUtilisateur == idUtilisateur)
                .OrderByDescending(m => m.DateAdhesion)
                .ToListAsync();

            var utilisateur = await _context.Utilisateurs.FindAsync(idUtilisateur);
            ViewBag.UtilisateurNom = utilisateur?.Email ?? "Utilisateur inconnu";

            return View(clubs);
        }

        private bool MembreClubExists(int idUtilisateur, int idClub)
        {
            return _context.MembreClubs.Any(e => e.IdUtilisateur == idUtilisateur && e.IdClub == idClub);
        }
    }
}