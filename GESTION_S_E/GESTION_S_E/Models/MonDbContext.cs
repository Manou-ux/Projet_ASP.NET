using GESTION_S_E.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjetAsp.Models
{
    public class MonDbContext : DbContext
    {
        public MonDbContext(DbContextOptions<MonDbContext> options) : base(options)
        {
        }

        // Ajoutez vos tables ici plus tard
        // public DbSet<Salle> Salles { get; set; }
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classe>()
                .HasCheckConstraint("CK_Classe_Niveau", "\"Niveau\" IN ('L1','L2','L3','M1','M2')");
            // Contrainte UNIQUE pour le matricule de l'élève
            modelBuilder.Entity<Eleve>()
                .HasIndex(e => e.Matricule)
                .IsUnique();

            // Contrainte UNIQUE pour id_utilisateur (relation 1-à-1)
            modelBuilder.Entity<Eleve>()
                .HasIndex(e => e.IdUtilisateur)
                .IsUnique();
            // Contrainte UNIQUE pour id_utilisateur de l'enseignant
            modelBuilder.Entity<Enseignant>()
                .HasIndex(e => e.IdUtilisateur)
                .IsUnique();
            // Contrainte UNIQUE pour id_utilisateur de la scolarité
            modelBuilder.Entity<Scolarite>()
                .HasIndex(s => s.IdUtilisateur)
                .IsUnique();
            // Configuration de la clé primaire composée
            modelBuilder.Entity<EleveGroupe>()
                .HasKey(eg => new { eg.IdEleve, eg.IdGroupe });

            // Optionnel : Si vous voulez que la suppression d'un élève supprime son lien avec le groupe
            modelBuilder.Entity<EleveGroupe>()
                .HasOne(eg => eg.Eleve)
                .WithMany()
                .HasForeignKey(eg => eg.IdEleve);

            modelBuilder.Entity<EleveGroupe>()
                .HasOne(eg => eg.Groupe)
                .WithMany()
                .HasForeignKey(eg => eg.IdGroupe);
            // Contrainte UNIQUE pour le code_matiere (ex: INF101)
            modelBuilder.Entity<Matiere>()
                .HasIndex(m => m.CodeMatiere)
                .IsUnique();
            // Configuration de la clé primaire composée pour Matiere_Classe
            modelBuilder.Entity<MatiereClasse>()
                .HasKey(mc => new { mc.IdMatiere, mc.IdClasse });
            // Nom de salle unique (ex: Salle 101)
            modelBuilder.Entity<Salle>()
                .HasIndex(s => s.NomSalle)
                .IsUnique();

            // Contrainte CHECK pour le type de salle
            modelBuilder.Entity<Salle>()
                .HasCheckConstraint("CK_Salle_Type", "\"type\" IN ('TP','cours','amphi','reunion')");
            // 1. Contrainte pour le Semestre (S1 à S6)
            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Semestre", "semestre IN ('S1','S2','S3','S4','S5','S6')");

            // 2. Contrainte pour le Statut
            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Statut", "statut IN ('planifie','en_cours','termine','annule','reporte')");

            // 3. Contrainte logique : id_classe ou id_groupe doit être rempli
            modelBuilder.Entity<EmploiDuTemps>()
                .HasCheckConstraint("CK_Emploi_Destinataire", "id_classe IS NOT NULL OR id_groupe IS NOT NULL");
            // 1. Contrainte pour les jours de la semaine
            modelBuilder.Entity<DisponibiliteEnseignant>()
                .HasCheckConstraint("CK_Dispo_Jour", "jour IN ('Lundi','Mardi','Mercredi','Jeudi','Vendredi','Samedi')");

            // 2. Contrainte pour le type de disponibilité
            modelBuilder.Entity<DisponibiliteEnseignant>()
                .HasCheckConstraint("CK_Dispo_Type", "type_dispo IN ('cours','td','tp','reunion')");
            // Nom du club unique (ex: Club Informatique)
            modelBuilder.Entity<Club>()
                .HasIndex(c => c.NomClub)
                .IsUnique();
            // Configuration de la clé primaire composée (Un utilisateur ne peut pas être deux fois membre du même club)
            modelBuilder.Entity<MembreClub>()
                .HasKey(mc => new { mc.IdUtilisateur, mc.IdClub });

            // Contrainte CHECK pour le rôle dans le club
            modelBuilder.Entity<MembreClub>()
                .HasCheckConstraint("CK_Membre_Role", "role_membre IN ('president','tresorier','secretaire','membre')");
            // Contrainte CHECK pour le statut de la réservation
            modelBuilder.Entity<ReservationSalle>()
                .HasCheckConstraint("CK_Reservation_Statut", "statut IN ('en_attente','validee','annulee')");
        }

    }
}