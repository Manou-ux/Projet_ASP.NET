using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GESTION_S_E.Controllers
{
    public class ClubsController : Controller
    {
        private readonly MonDbContext _context;

        public ClubsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Clubs (version modifiée pour afficher le nom complet du responsable)
        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Responsable)
                .OrderByDescending(c => c.DateCreation)
                .Select(c => new ClubWithResponsableName
                {
                    IdClub = c.IdClub,
                    NomClub = c.NomClub,
                    Description = c.Description,
                    DateCreation = c.DateCreation,
                    Actif = c.Actif,
                    ResponsableId = c.IdResponsable,
                    ResponsableNomComplet = c.Responsable == null ? "Non assigné" :
                        (c.Responsable.Role == "eleve" ?
                            _context.Eleves.Where(e => e.IdUtilisateur == c.Responsable.IdUtilisateur)
                                .Select(e => e.PrenomEleve + " " + e.NomEleve).FirstOrDefault() :
                         c.Responsable.Role == "enseignant" ?
                            _context.Enseignants.Where(e => e.IdUtilisateur == c.Responsable.IdUtilisateur)
                                .Select(e => e.PrenomEnseignant + " " + e.NomEnseignant).FirstOrDefault() :
                         c.Responsable.Role == "scolarite" ?
                            _context.Scolarites.Where(s => s.IdUtilisateur == c.Responsable.IdUtilisateur)
                                .Select(s => s.PrenomScolarite + " " + s.NomScolarite).FirstOrDefault() :
                         c.Responsable.Email)
                })
                .ToListAsync();

            return View(clubs);
        }

        // GET: Clubs/Create
        public async Task<IActionResult> Create()
        {
            await LoadResponsablesList();
            return View();
        }

        // POST: Clubs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            ModelState.Remove("Responsable");

            if (ModelState.IsValid)
            {
                try
                {
                    club.DateCreation = DateTime.UtcNow;
                    club.Actif = true;

                    _context.Clubs.Add(club);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Le club '{club.NomClub}' a été créé avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur : {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            await LoadResponsablesList(club.IdResponsable);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                TempData["Error"] = "Club non trouvé";
                return RedirectToAction(nameof(Index));
            }

            await LoadResponsablesList(club.IdResponsable);
            return View(club);
        }

        // POST: Clubs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdClub,NomClub,Description,IdResponsable,Actif")] Club club)
        {
            if (id != club.IdClub) return NotFound();

            ModelState.Remove("Responsable");
            ModelState.Remove("DateCreation");

            if (ModelState.IsValid)
            {
                try
                {
                    var clubExistant = await _context.Clubs.FindAsync(id);
                    if (clubExistant == null) return NotFound();

                    clubExistant.NomClub = club.NomClub;
                    clubExistant.Description = club.Description;
                    clubExistant.IdResponsable = club.IdResponsable;
                    clubExistant.Actif = club.Actif;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Club modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.IdClub)) return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur : {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            await LoadResponsablesList(club.IdResponsable);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.Responsable)
                .FirstOrDefaultAsync(c => c.IdClub == id);

            if (club == null)
            {
                TempData["Error"] = "Club non trouvé";
                return RedirectToAction(nameof(Index));
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var club = await _context.Clubs.FindAsync(id);
                if (club != null)
                {
                    _context.Clubs.Remove(club);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = $"Le club '{club.NomClub}' a été supprimé.";
                }
                else
                {
                    TempData["Error"] = "Club non trouvé";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur : {ex.InnerException?.Message ?? ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ============================================================
        // GESTION DES MEMBRES
        // ============================================================

        // GET: Clubs/Members/5
        public async Task<IActionResult> Members(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                TempData["Error"] = "Club non trouvé";
                return RedirectToAction(nameof(Index));
            }

            // Récupérer les membres avec leur nom complet (calculé en SQL)
            var membres = await _context.MembreClubs
                .Include(m => m.Utilisateur)
                .Where(m => m.IdClub == id)
                .Select(m => new MembreViewModel
                {
                    IdMembre = m.IdMembre,
                    IdUtilisateur = m.IdUtilisateur,
                    RoleMembre = m.RoleMembre,
                    DateAdhesion = m.DateAdhesion,
                    NomComplet = (m.Utilisateur.Role == "eleve" ?
                                    _context.Eleves.Where(e => e.IdUtilisateur == m.Utilisateur.IdUtilisateur)
                                        .Select(e => e.PrenomEleve + " " + e.NomEleve).FirstOrDefault() :
                                  m.Utilisateur.Role == "enseignant" ?
                                    _context.Enseignants.Where(e => e.IdUtilisateur == m.Utilisateur.IdUtilisateur)
                                        .Select(e => e.PrenomEnseignant + " " + e.NomEnseignant).FirstOrDefault() :
                                  m.Utilisateur.Role == "scolarite" ?
                                    _context.Scolarites.Where(s => s.IdUtilisateur == m.Utilisateur.IdUtilisateur)
                                        .Select(s => s.PrenomScolarite + " " + s.NomScolarite).FirstOrDefault() :
                                  m.Utilisateur.Email)
                })
                .ToListAsync();

            // Liste des utilisateurs disponibles (non encore membres)
            var utilisateursDisponibles = await _context.Utilisateurs
                .Where(u => !_context.MembreClubs.Any(m => m.IdClub == id && m.IdUtilisateur == u.IdUtilisateur))
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = (u.Role == "eleve" ?
                                    _context.Eleves.Where(e => e.IdUtilisateur == u.IdUtilisateur)
                                        .Select(e => e.PrenomEleve + " " + e.NomEleve).FirstOrDefault() :
                                  u.Role == "enseignant" ?
                                    _context.Enseignants.Where(e => e.IdUtilisateur == u.IdUtilisateur)
                                        .Select(e => e.PrenomEnseignant + " " + e.NomEnseignant).FirstOrDefault() :
                                  u.Role == "scolarite" ?
                                    _context.Scolarites.Where(s => s.IdUtilisateur == u.IdUtilisateur)
                                        .Select(s => s.PrenomScolarite + " " + s.NomScolarite).FirstOrDefault() :
                                  u.Email)
                })
                .ToListAsync();

            ViewBag.Club = club;
            ViewBag.UtilisateursDisponibles = utilisateursDisponibles;
            return View(membres);
        }

        // POST: Clubs/AddMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(int clubId, int idUtilisateur, string roleMembre)
        {
            try
            {
                var existe = await _context.MembreClubs
                    .AnyAsync(m => m.IdClub == clubId && m.IdUtilisateur == idUtilisateur);
                if (existe)
                {
                    TempData["Error"] = "Cet utilisateur est déjà membre.";
                    return RedirectToAction(nameof(Members), new { id = clubId });
                }

                // Liste des rôles autorisés par la contrainte CK_Membre_Role
                var rolesAutorises = new[] { "membre", "vice-president", "president", "secretaire", "tresorier" };
                string roleFinal = string.IsNullOrWhiteSpace(roleMembre) ? "membre" : roleMembre.ToLower();
                if (!rolesAutorises.Contains(roleFinal))
                {
                    roleFinal = "membre";  // valeur par défaut
                }

                var membre = new MembreClub
                {
                    IdClub = clubId,
                    IdUtilisateur = idUtilisateur,
                    RoleMembre = roleFinal,
                    DateAdhesion = DateTime.Now
                };

                _context.MembreClubs.Add(membre);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Membre ajouté avec succès.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur : {ex.InnerException?.Message ?? ex.Message}";
            }
            return RedirectToAction(nameof(Members), new { id = clubId });
        }

        // POST: Clubs/RemoveMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMember(int idMembre, int clubId)
        {
            try
            {
                // Recherche par la colonne IdMembre (même si ce n'est pas la PK)
                var membre = await _context.MembreClubs.FirstOrDefaultAsync(m => m.IdMembre == idMembre);
                if (membre != null)
                {
                    _context.MembreClubs.Remove(membre);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Membre retiré du club.";
                }
                else
                {
                    TempData["Error"] = "Membre non trouvé.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur : {ex.InnerException?.Message ?? ex.Message}";
            }
            return RedirectToAction(nameof(Members), new { id = clubId });
        }

        // ============================================================
        // HELPERS
        // ============================================================

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.IdClub == id);
        }

        private async Task LoadResponsablesList(object selectedValue = null)
        {
            var utilisateurs = await _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    u.Role,
                    NomComplet = (u.Role == "eleve" ?
                                    _context.Eleves.Where(e => e.IdUtilisateur == u.IdUtilisateur)
                                        .Select(e => e.PrenomEleve + " " + e.NomEleve).FirstOrDefault() :
                                  u.Role == "enseignant" ?
                                    _context.Enseignants.Where(e => e.IdUtilisateur == u.IdUtilisateur)
                                        .Select(e => e.PrenomEnseignant + " " + e.NomEnseignant).FirstOrDefault() :
                                  u.Role == "scolarite" ?
                                    _context.Scolarites.Where(s => s.IdUtilisateur == u.IdUtilisateur)
                                        .Select(s => s.PrenomScolarite + " " + s.NomScolarite).FirstOrDefault() :
                                  u.Email)
                })
                .ToListAsync();

            var listeFinale = utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = string.IsNullOrWhiteSpace(u.NomComplet) ? u.Role : u.NomComplet
                })
                .OrderBy(u => u.NomComplet)
                .ToList();

            ViewBag.IdResponsable = new SelectList(listeFinale, "IdUtilisateur", "NomComplet", selectedValue);
        }

        // ViewModel pour l'affichage des membres
        public class MembreViewModel
        {
            public int IdMembre { get; set; }
            public int IdUtilisateur { get; set; }  // Ajouté pour la suppression
            public string RoleMembre { get; set; }
            public DateTime DateAdhesion { get; set; }
            public string NomComplet { get; set; }
        }

        // ViewModel pour l'affichage des clubs avec le nom complet du responsable
        public class ClubWithResponsableName
        {
            public int IdClub { get; set; }
            public string NomClub { get; set; }
            public string Description { get; set; }
            public DateTime DateCreation { get; set; }
            public bool Actif { get; set; }
            public int ResponsableId { get; set; }
            public string ResponsableNomComplet { get; set; }
        }
    }
}