using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("emplois_du_temps")]
    public class EmploiDuTemps
    {
        [Key]
        [Column("id_emploi")]
        public int IdEmploi { get; set; }

        [Required]
        [Column("date_cours")]
        public DateTime DateCours { get; set; }

        [Required]
        [Column("heure_debut")]
        public TimeSpan HeureDebut { get; set; }

        [Required]
        [Column("heure_fin")]
        public TimeSpan HeureFin { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("semestre")]
        public string Semestre { get; set; }

        [Required]
        [MaxLength(15)]
        [Column("statut")]
        public string Statut { get; set; } = "planifie";

        // --- RELATIONS ---

        [Column("id_salle")]
        public int IdSalle { get; set; }
        [ForeignKey("IdSalle")]
        public virtual Salle Salle { get; set; }

        [Column("id_enseignant")]
        public int IdEnseignant { get; set; }
        [ForeignKey("IdEnseignant")]
        public virtual Enseignant Enseignant { get; set; }

        [Column("id_matiere")]
        public int IdMatiere { get; set; }
        [ForeignKey("IdMatiere")]
        public virtual Matiere Matiere { get; set; }

        [Column("id_classe")]
        public int? IdClasse { get; set; } // Nullable car on peut viser un groupe à la place
        [ForeignKey("IdClasse")]
        public virtual Classe Classe { get; set; }

        [Column("id_groupe")]
        public int? IdGroupe { get; set; } // Nullable car on peut viser une classe entière à la place
        [ForeignKey("IdGroupe")]
        public virtual Groupe Groupe { get; set; }
    }
}
