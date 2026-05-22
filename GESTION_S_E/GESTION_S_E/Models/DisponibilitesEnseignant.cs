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
        [Required(ErrorMessage = "L'enseignant est obligatoire")]
        public int IdEnseignant { get; set; }

        [ForeignKey("IdEnseignant")]
        public virtual Enseignant Enseignant { get; set; } = null!;

        [Required(ErrorMessage = "Le jour est obligatoire")]
        [MaxLength(10)]
        [Column("jour")]
        public string Jour { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        [Column("heure_debut")]
        [Display(Name = "Heure début")]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        [Column("heure_fin")]
        [Display(Name = "Heure fin")]
        public TimeSpan HeureFin { get; set; }

        [Required(ErrorMessage = "Le type est obligatoire")]
        [MaxLength(10)]
        [Column("type_dispo")]
        public string TypeDispo { get; set; } = "cours";

        [Column("date_specifique")]
        [Display(Name = "Date spécifique")]
        public DateTime? DateSpecifique { get; set; }

        // Validation personnalisée (à utiliser dans le controller)
        public bool IsValidTimeRange()
        {
            return HeureFin > HeureDebut;
        }
    }
}