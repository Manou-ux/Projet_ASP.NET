using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class ClubsController : Controller
    {
        private readonly MonDbContext _context;

        public ClubsController(MonDbContext context)
        {
            _context = context;
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Responsable)
                .OrderByDescending(c => c.DateCreation)
                .ToListAsync();
            return View(clubs);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            // Afficher le nom au lieu de l'email
            var utilisateurs = _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = (u.Role == "eleve" ? _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEleve + " " + _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEleve :
                                u.Role == "enseignant" ? _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEnseignant + " " + _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEnseignant :
                                u.Role == "scolarite" ? _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).NomScolarite + " " + _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).PrenomScolarite :
                                u.Email)
                })
                .ToList();

            ViewBag.IdResponsable = new SelectList(utilisateurs, "IdUtilisateur", "NomComplet");
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
                    var innerException = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ModelState.AddModelError("", $"Erreur technique : {innerException}");
                    System.Diagnostics.Debug.WriteLine($"Erreur complète: {ex.ToString()}");
                }
            }

            var utilisateurs = _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = (u.Role == "eleve" ? _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEleve + " " + _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEleve :
                                u.Role == "enseignant" ? _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEnseignant + " " + _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEnseignant :
                                u.Role == "scolarite" ? _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).NomScolarite + " " + _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).PrenomScolarite :
                                u.Email)
                })
                .ToList();

            ViewBag.IdResponsable = new SelectList(utilisateurs, "IdUtilisateur", "NomComplet", club.IdResponsable);
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                TempData["Error"] = $"Club avec ID {id} non trouvé";
                return RedirectToAction(nameof(Index));
            }

            var utilisateurs = _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = (u.Role == "eleve" ? _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEleve + " " + _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEleve :
                                u.Role == "enseignant" ? _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEnseignant + " " + _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEnseignant :
                                u.Role == "scolarite" ? _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).NomScolarite + " " + _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).PrenomScolarite :
                                u.Email)
                })
                .ToList();

            ViewBag.IdResponsable = new SelectList(utilisateurs, "IdUtilisateur", "NomComplet", club.IdResponsable);
            return View(club);
        }

        // POST: Clubs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdClub,NomClub,Description,IdResponsable,Actif")] Club club)
        {
            if (id != club.IdClub)
            {
                return NotFound();
            }

            // Retirer la validation de Responsable car c'est une propriété de navigation
            ModelState.Remove("Responsable");
            ModelState.Remove("DateCreation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Récupérer le club existant pour conserver la date de création
                    var clubExistant = await _context.Clubs.FindAsync(id);
                    if (clubExistant == null)
                    {
                        return NotFound();
                    }

                    // Mettre à jour uniquement les champs modifiables
                    clubExistant.NomClub = club.NomClub;
                    clubExistant.Description = club.Description;
                    clubExistant.IdResponsable = club.IdResponsable;
                    clubExistant.Actif = club.Actif;

                    _context.Update(clubExistant);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Club modifié avec succès !";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.IdClub))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de la modification : {ex.InnerException?.Message ?? ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Erreur modification: {ex.ToString()}");
                }
            }

            // Si on arrive ici, il y a une erreur de validation
            var utilisateurs = _context.Utilisateurs
                .Select(u => new
                {
                    u.IdUtilisateur,
                    NomComplet = (u.Role == "eleve" ? _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEleve + " " + _context.Eleves.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEleve :
                                u.Role == "enseignant" ? _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).NomEnseignant + " " + _context.Enseignants.FirstOrDefault(e => e.IdUtilisateur == u.IdUtilisateur).PrenomEnseignant :
                                u.Role == "scolarite" ? _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).NomScolarite + " " + _context.Scolarites.FirstOrDefault(s => s.IdUtilisateur == u.IdUtilisateur).PrenomScolarite :
                                u.Email)
                })
                .ToList();

            ViewBag.IdResponsable = new SelectList(utilisateurs, "IdUtilisateur", "NomComplet", club.IdResponsable);
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
                    TempData["Success"] = $"Le club '{club.NomClub}' a été supprimé avec succès.";
                }
                else
                {
                    TempData["Error"] = "Club non trouvé";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur lors de la suppression : {ex.InnerException?.Message ?? ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Erreur suppression: {ex.ToString()}");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.IdClub == id);
        }
    }
}