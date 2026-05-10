using Microsoft.AspNetCore.Mvc;
using GESTION_S_E.Models;

namespace GESTION_S_E.Controllers
{
    public class SallesController : Controller
    {
        private readonly MonDbContext _context;

        public SallesController(MonDbContext context)
        {
            _context = context;
        }

        // LISTE
        public IActionResult Index()
        {
            var salles = _context.Salles.ToList();
            return View(salles);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string NomSalle, int Capacite, string Type, string Localisation, bool Disponible)
        {
            var salle = new Salle
            {
                NomSalle = NomSalle,
                Capacite = Capacite,
                Type = Type,
                Localisation = Localisation,
                Disponible = Disponible
            };

            _context.Salles.Add(salle);
            _context.SaveChanges();
            TempData["Success"] = "Salle ajoutée avec succès!";

            return RedirectToAction("Index");
        }

        // EDIT GET - Afficher le formulaire de modification
        public IActionResult Edit(int id)
        {
            var salle = _context.Salles.Find(id);
            if (salle == null)
            {
                return NotFound();
            }
            return View(salle);
        }

        // EDIT POST - Enregistrer les modifications
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string NomSalle, int Capacite, string Type, string Localisation, bool Disponible)
        {
            var salle = _context.Salles.Find(id);
            if (salle == null)
            {
                return NotFound();
            }

            salle.NomSalle = NomSalle;
            salle.Capacite = Capacite;
            salle.Type = Type;
            salle.Localisation = Localisation;
            salle.Disponible = Disponible;

            _context.Salles.Update(salle);
            _context.SaveChanges();
            TempData["Success"] = "Salle modifiée avec succès!";

            return RedirectToAction("Index");
        }

        // DELETE GET - Afficher la confirmation
        public IActionResult Delete(int id)
        {
            var salle = _context.Salles.Find(id);
            if (salle == null)
            {
                return NotFound();
            }
            return View(salle);
        }

        // DELETE POST - Supprimer la salle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var salle = _context.Salles.Find(id);
            if (salle != null)
            {
                _context.Salles.Remove(salle);
                _context.SaveChanges();
                TempData["Success"] = "Salle supprimée avec succès!";
            }

            return RedirectToAction("Index");
        }
    }
}