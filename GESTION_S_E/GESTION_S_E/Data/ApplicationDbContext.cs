using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Eleve> Eleves { get; set; }
        public DbSet<Enseignant> Enseignants { get; set; }
        public DbSet<Scolarite> Scolarites { get; set; }
        public DbSet<Groupe> Groupes { get; set; }
        public DbSet<EleveGroupe> EleveGroupes { get; set; }
        public DbSet<Matiere> Matieres { get; set; }
        public DbSet<MatiereClasse> MatiereClasses { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<EmploiDuTemps> EmploisDuTemps { get; set; }
        public DbSet<DisponibiliteEnseignant> DisponibilitesEnseignants { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<MembreClub> MembresClubs { get; set; }
        public DbSet<ReservationSalle> ReservationsSalles { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des clés composées
            modelBuilder.Entity<EleveGroupe>()
                .HasKey(eg => new { eg.IdEleve, eg.IdGroupe });

            modelBuilder.Entity<MatiereClasse>()
                .HasKey(mc => new { mc.IdMatiere, mc.IdClasse });

            modelBuilder.Entity<MembreClub>()
                .HasKey(mc => new { mc.IdUtilisateur, mc.IdClub });
        }
    }
}