using BCrypt.Net;
using GESTION_S_E.Models;
using GESTION_S_E.Services;
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
        private readonly IEmailSender _emailSender;

        public AccountController(MonDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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

            if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
            {
                ViewBag.Error = "Cet email est déjà utilisé";
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem { Value = c.IdClasse.ToString(), Text = $"{c.NomClasse} - {c.Filiere}" }).ToList();
                return View();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
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

        // ==================== MOT DE PASSE OUBLIÉ (avec logs détaillés) ====================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            Console.WriteLine("=== FORGOT PASSWORD POST ===");

            // Log toutes les clés du formulaire
            Console.WriteLine("=== TOUTES LES CLÉS DU FORMULAIRE ===");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"Clé : {key} = {Request.Form[key]}");
            }

            // Log des différentes sources de l'email
            var emailFromForm = Request.Form["Email"].ToString();
            var emailFromModel = model.Email;
            Console.WriteLine($"Email reçu (Request.Form) : {emailFromForm}");
            Console.WriteLine($"Email reçu (model) : {emailFromModel}");

            // Utiliser la première valeur non vide
            var email = !string.IsNullOrEmpty(emailFromForm) ? emailFromForm : emailFromModel;
            Console.WriteLine($"Email utilisé : {email}");

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("ERREUR : Email vide");
                ModelState.AddModelError("Email", "L'adresse email est requise.");
                return View(new ForgotPasswordViewModel());
            }

            // Rechercher l'utilisateur
            Console.WriteLine($"Recherche de l'utilisateur avec email : {email}");
            var user = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                Console.WriteLine($"Utilisateur non trouvé pour {email}");
                TempData["Success"] = "Si cet email existe, un lien de réinitialisation vous a été envoyé.";
                return RedirectToAction(nameof(Login));
            }
            Console.WriteLine($"Utilisateur trouvé : {user.Email} (ID: {user.IdUtilisateur})");

            // Générer le token
            var token = Guid.NewGuid().ToString();
            Console.WriteLine($"Token généré : {token}");

            var resetToken = new PasswordResetToken
            {
                IdUtilisateur = user.IdUtilisateur,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(24),
                Used = false
            };
            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();
            Console.WriteLine("Token sauvegardé en base.");

            // Construire le lien
            var resetLink = Url.Action("ResetPassword", "Account", new { token }, Request.Scheme);
            Console.WriteLine($"Lien de réinitialisation : {resetLink}");

            var subject = "Réinitialisation de votre mot de passe";
            var body = $"<p>Bonjour,</p><p>Cliquez sur le lien ci-dessous pour réinitialiser votre mot de passe :</p><p><a href='{resetLink}'>{resetLink}</a></p><p>Ce lien est valable 24h.</p>";

            // Envoyer l'email
            try
            {
                Console.WriteLine($"Tentative d'envoi d'email à {user.Email}");
                await _emailSender.SendEmailAsync(user.Email, subject, body);
                Console.WriteLine($"Email envoyé avec succès à {user.Email}");
                TempData["Success"] = "Un lien de réinitialisation a été envoyé à votre adresse email.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'email : {ex.Message}");
                TempData["Error"] = "Une erreur est survenue lors de l'envoi de l'email. Veuillez réessayer.";
                return View(new ForgotPasswordViewModel());
            }

            return RedirectToAction(nameof(Login));
        }

        // ==================== RESET PASSWORD ====================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token manquant.";
                return RedirectToAction(nameof(Login));
            }

            var resetToken = _context.PasswordResetTokens
                .Include(t => t.Utilisateur)
                .FirstOrDefault(t => t.Token == token && !t.Used && t.ExpirationDate > DateTime.UtcNow);

            if (resetToken == null)
            {
                TempData["Error"] = "Lien invalide ou expiré.";
                return RedirectToAction(nameof(Login));
            }

            var model = new ResetPasswordViewModel { Token = token };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var resetToken = await _context.PasswordResetTokens
                .Include(t => t.Utilisateur)
                .FirstOrDefaultAsync(t => t.Token == model.Token && !t.Used && t.ExpirationDate > DateTime.UtcNow);

            if (resetToken == null)
            {
                TempData["Error"] = "Lien invalide ou expiré.";
                return RedirectToAction(nameof(Login));
            }

            var user = resetToken.Utilisateur;
            user.MotDePasse = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            resetToken.Used = true;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Votre mot de passe a été réinitialisé avec succès.";
            return RedirectToAction(nameof(Login));
        }

        // ==================== LOGOUT ====================
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ==================== TEST EMAIL (temporaire) ====================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TestEmail()
        {
            try
            {
                await _emailSender.SendEmailAsync(
                    "votre.email.pour.test@gmail.com",  // remplacez par votre adresse email
                    "Test SMTP",
                    "<p>Ceci est un email de test</p>"
                );
                return Content("✅ Email envoyé avec succès !");
            }
            catch (Exception ex)
            {
                return Content($"❌ Erreur : {ex.Message}");
            }
        }

        // ==================== MÉTHODES PRIVÉES ====================
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
    }
}