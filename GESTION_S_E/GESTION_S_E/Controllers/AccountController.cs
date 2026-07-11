using BCrypt.Net;
using GESTION_S_E.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GESTION_S_E.Controllers
{
    public class AccountController : Controller
    {
        private readonly MonDbContext _context;

        public AccountController(MonDbContext context)
        {
            _context = context;
        }

        // ==================== LOGIN ====================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            // Comptes de test
            if (email == "admin@emit.sn" && password == "admin123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "admin"),
                    new Claim("UserId", "1"),
                    new Claim("FullName", "Administrateur EMIT")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }
            else if (email == "enseignant@emit.sn" && password == "enseignant123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "enseignant"),
                    new Claim("UserId", "2"),
                    new Claim("FullName", "Professeur EMIT")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }
            else if (email == "eleve@emit.sn" && password == "eleve123")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "eleve"),
                    new Claim("UserId", "3"),
                    new Claim("FullName", "Étudiant EMIT")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }

            // Vérifier dans la base de données
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == email && u.Actif);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.MotDePasse))
            {
                var fullName = await GetUserFullNameAsync(user);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.IdUtilisateur.ToString()),
                    new Claim("FullName", fullName)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Email ou mot de passe incorrect";
            return View();
        }

        // ==================== REGISTER ====================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewBag.Classes = _context.Classes
                .Select(c => new SelectListItem
                {
                    Value = c.IdClasse.ToString(),
                    Text = $"{c.NomClasse} - {c.Filiere}"
                })
                .ToList();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword,
            string nom, string prenom, string role, string telephone,
            string matricule, int? idClasse, string specialite, string fonction, string bureau)
        {
            // Validation manuelle
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "Tous les champs obligatoires doivent être remplis";
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Les mots de passe ne correspondent pas";
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                return View();
            }

            // Vérifier si l'email existe déjà
            if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
            {
                ViewBag.Error = "Cet email est déjà utilisé";
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                return View();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Créer l'utilisateur
                var utilisateur = new Utilisateur
                {
                    Email = email,
                    MotDePasse = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = role,
                    Actif = true,
                    DateCreation = DateTime.UtcNow
                };

                _context.Utilisateurs.Add(utilisateur);
                await _context.SaveChangesAsync();

                // Créer l'entité spécifique selon le rôle
                switch (role)
                {
                    case "eleve":
                        var eleve = new Eleve
                        {
                            NomEleve = nom,
                            PrenomEleve = prenom,
                            Matricule = matricule ?? $"ELEV{DateTime.Now.Ticks}",
                            Telephone = telephone ?? "",
                            IdClasse = idClasse ?? 1,
                            IdUtilisateur = utilisateur.IdUtilisateur
                        };
                        _context.Eleves.Add(eleve);
                        break;

                    case "enseignant":
                        var enseignant = new Enseignant
                        {
                            NomEnseignant = nom,
                            PrenomEnseignant = prenom,
                            Specialite = specialite ?? "",
                            TelephoneEnseignant = telephone ?? "",
                            EmailPro = email,
                            IdUtilisateur = utilisateur.IdUtilisateur
                        };
                        _context.Enseignants.Add(enseignant);
                        break;

                    case "scolarite":
                        var scolarite = new Scolarite
                        {
                            NomScolarite = nom,
                            PrenomScolarite = prenom,
                            Fonction = fonction ?? "Agent de scolarité",
                            Telephone = telephone ?? "",
                            Bureau = bureau ?? "",
                            IdUtilisateur = utilisateur.IdUtilisateur
                        };
                        _context.Scolarites.Add(scolarite);
                        break;

                    default:
                        ViewBag.Error = "Rôle non valide";
                        await transaction.RollbackAsync();
                        ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                        return View();
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ViewBag.Error = $"Erreur lors de l'inscription : {ex.Message}";
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                return View();
            }
        }
        // ==================== PROFILE ====================

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var user = await _context.Utilisateurs.FindAsync(userId);

            if (user == null)
                return RedirectToAction("Login");

            var fullName = await GetUserFullNameAsync(user);

            ViewBag.Email = user.Email;
            ViewBag.Role = user.Role;
            ViewBag.FullName = fullName;
            ViewBag.Nom = fullName.Split(' ').Length > 1 ? fullName.Split(' ')[^1] : "";
            ViewBag.Prenom = fullName.Split(' ').Length > 1 ? string.Join(" ", fullName.Split(' ')[..^1]) : fullName;
            ViewBag.DateCreation = user.DateCreation;

            return View();
        }

        private async Task<string> GetUserFullNameAsync(Utilisateur user)
        {
            return user.Role switch
            {
                "eleve" => await GetFullNameFromEleveAsync(user.IdUtilisateur),
                "enseignant" => await GetFullNameFromEnseignantAsync(user.IdUtilisateur),
                "scolarite" => await GetFullNameFromScolariteAsync(user.IdUtilisateur),
                _ => user.Email
            };
        }

        private async Task<string> GetFullNameFromEleveAsync(int userId)
        {
            var eleve = await _context.Eleves.AsNoTracking().FirstOrDefaultAsync(e => e.IdUtilisateur == userId);
            return BuildFullName(eleve?.PrenomEleve, eleve?.NomEleve);
        }

        private async Task<string> GetFullNameFromEnseignantAsync(int userId)
        {
            var enseignant = await _context.Enseignants.AsNoTracking().FirstOrDefaultAsync(e => e.IdUtilisateur == userId);
            return BuildFullName(enseignant?.PrenomEnseignant, enseignant?.NomEnseignant);
        }

        private async Task<string> GetFullNameFromScolariteAsync(int userId)
        {
            var scolarite = await _context.Scolarites.AsNoTracking().FirstOrDefaultAsync(s => s.IdUtilisateur == userId);
            return BuildFullName(scolarite?.PrenomScolarite, scolarite?.NomScolarite);
        }

        private static string BuildFullName(string? prenom, string? nom)
        {
            var parts = new List<string?> { prenom, nom };
            var fullName = string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
            return string.IsNullOrWhiteSpace(fullName) ? string.Empty : fullName;
        }

        // ==================== LOGOUT ====================

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}