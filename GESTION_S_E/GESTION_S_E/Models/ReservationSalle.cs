using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("reservations_salle")]
    public class ReservationSalle
    {
        [Key]
        [Column("id_reservation")]
        public int IdReservation { get; set; }

        [Column("id_salle")]
        [Required(ErrorMessage = "La salle est obligatoire")]
        public int IdSalle { get; set; }

        [ForeignKey("IdSalle")]
        public virtual Salle Salle { get; set; } = null!;

        [Column("id_utilisateur")]
        [Required(ErrorMessage = "Le demandeur est obligatoire")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; } = null!;

        [Column("id_club")]
        public int? IdClub { get; set; }

        [ForeignKey("IdClub")]
        public virtual Club? Club { get; set; }

        [Column("date_reservation")]
        public DateTime DateReservation { get; set; }

        [Required(ErrorMessage = "L'heure de début est obligatoire")]
        [Column("heure_debut")]
        [Display(Name = "Heure début")]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "L'heure de fin est obligatoire")]
        [Column("heure_fin")]
        [Display(Name = "Heure fin")]
        public TimeSpan HeureFin { get; set; }

        [Required(ErrorMessage = "Le motif est obligatoire")]
        [Column("motif")]
        public string Motif { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        [Column("statut")]
        public string Statut { get; set; } = "en_attente";

        // Validation personnalisée
        public bool IsValidTimeRange()
        {
            return HeureFin > HeureDebut;
        }
    }
}