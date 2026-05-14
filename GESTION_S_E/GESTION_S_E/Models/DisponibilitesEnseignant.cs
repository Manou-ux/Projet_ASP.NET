using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("disponibilites_enseignants")]
    public class DisponibiliteEnseignant
    {
        [Key]
        [Column("id_dispo")]
        public int IdDispo { get; set; }

        [Column("id_enseignant")]
        public int IdEnseignant { get; set; }

        [ForeignKey("IdEnseignant")]
        public virtual Enseignant Enseignant { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("jour")]
        public string Jour { get; set; }

        [Required]
        [Column("heure_debut")]
        public TimeSpan HeureDebut { get; set; }

        [Required]
        [Column("heure_fin")]
        public TimeSpan HeureFin { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("type_dispo")]
        public string TypeDispo { get; set; } = "cours";

        [Column("date_specifique")]
        public DateTime? DateSpecifique { get; set; }
    }
}