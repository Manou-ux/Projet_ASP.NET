using Microsoft.EntityFrameworkCore;
using GESTION_S_E.Models;

namespace GESTION_S_E.Models
{
    public class MonDbContext : DbContext
    {
        public MonDbContext(DbContextOptions<MonDbContext> options) : base(options)
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
        public DbSet<MembreClub> MembreClubs { get; set; }
        public DbSet<ReservationSalle> ReservationsSalles { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // ⭐ NOUVEAU : pour les tokens de réinitialisation
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==================== CONFIGURATION DE PasswordResetToken ====================
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasOne(t => t.Utilisateur)
                      .WithMany()
                      .HasForeignKey(t => t.IdUtilisateur)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(t => t.Token)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(t => t.ExpirationDate)
                      .HasColumnType("timestamp with time zone");

                entity.HasIndex(t => t.Token)
                      .IsUnique();

                // Optionnel : index pour les tokens non utilisés et non expirés
                entity.HasIndex(t => new { t.Token, t.Used, t.ExpirationDate });
            });

            // ==================== DISPONIBILITÉS ENSEIGNANTS ====================
            modelBuilder.Entity<DisponibiliteEnseignant>(entity =>
            {
                entity.HasKey(d => d.IdDispo);

                entity.HasOne(d => d.Enseignant)
                      .WithMany()
                      .HasForeignKey(d => d.IdEnseignant)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(d => d.HeureDebut)
                      .HasColumnType("interval");

                entity.Property(d => d.HeureFin)
                      .HasColumnType("interval");

                entity.Property(d => d.DateSpecifique)
                      .HasColumnType("timestamp with time zone");

                entity.HasCheckConstraint("CK_Dispo_Jour", "jour IN ('Lundi','Mardi','Mercredi','Jeudi','Vendredi','Samedi')");
                entity.HasCheckConstraint("CK_Dispo_Type", "type_dispo IN ('cours','td','tp','reunion')");
            });

            // ==================== RÉSERVATIONS SALLE ====================
            modelBuilder.Entity<ReservationSalle>(entity =>
            {
                entity.HasKey(r => r.IdReservation);

                entity.HasOne(r => r.Salle)
                      .WithMany()
                      .HasForeignKey(r => r.IdSalle)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Utilisateur)
                      .WithMany()
                      .HasForeignKey(r => r.IdUtilisateur)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Club)
                      .WithMany()
                      .HasForeignKey(r => r.IdClub)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.Property(r => r.DateReservation)
                      .HasColumnType("timestamp with time zone");

                entity.Property(r => r.HeureDebut)
                      .HasColumnType("interval");

                entity.Property(r => r.HeureFin)
                      .HasColumnType("interval");

                entity.HasCheckConstraint("CK_Reservation_Statut", "statut IN ('en_attente','validee','annulee')");
            });

            // ==================== TES CONFIGURATIONS ORIGINALES (conservées) ====================
            modelBuilder.Entity<Classe>()
                .HasCheckConstraint("CK_Classe_Niveau", "\"Niveau\" IN ('L1','L2','L3','M1','M2')");

            modelBuilder.Entity<Eleve>()
                .HasIndex(e => e.Matricule)
                .IsUnique();

            modelBuilder.Entity<Eleve>()
                .HasIndex(e => e.IdUtilisateur)
                .IsUnique();

            modelBuilder.Entity<Enseignant>()
                .HasIndex(e => e.IdUtilisateur)
                .IsUnique();

            modelBuilder.Entity<Scolarite>()
                .HasIndex(s => s.IdUtilisateur)
                .IsUnique();

            modelBuilder.Entity<EleveGroupe>()
                .HasKey(eg => new { eg.IdEleve, eg.IdGroupe });

            modelBuilder.Entity<EleveGroupe>()
                .HasOne(eg => eg.Eleve)
                .WithMany()
                .HasForeignKey(eg => eg.IdEleve);

            modelBuilder.Entity<EleveGroupe>()
                .HasOne(eg => eg.Groupe)
                .WithMany()
                .HasForeignKey(eg => eg.IdGroupe);

            modelBuilder.Entity<Matiere>()
                .HasIndex(m => m.CodeMatiere)
                .IsUnique();

            modelBuilder.Entity<MatiereClasse>()
                .HasKey(mc => new { mc.IdMatiere, mc.IdClasse });

            modelBuilder.Entity<Salle>()
                .HasIndex(s => s.NomSalle)
                .IsUnique();

            modelBuilder.Entity<Salle>()
                .HasCheckConstraint("CK_Salle_Type", "\"type\" IN ('TP','cours','amphi','reunion')");

            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Semestre", "semestre IN ('S1','S2','S3','S4','S5','S6')");

            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Statut", "statut IN ('planifie','en_cours','termine','annule','reporte')");

            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Destinataire", "id_classe IS NOT NULL OR id_groupe IS NOT NULL");

            modelBuilder.Entity<Club>()
                .HasIndex(c => c.NomClub)
                .IsUnique();

            modelBuilder.Entity<MembreClub>()
                .HasKey(mc => new { mc.IdUtilisateur, mc.IdClub });

            modelBuilder.Entity<MembreClub>()
                .HasCheckConstraint("CK_Membre_Role", "role_membre IN ('president','tresorier','secretaire','membre')");
        }
    }
}