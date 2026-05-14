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

        [Column("date_reservation")]
        public DateTime DateReservation { get; set; }

        [Required]
        [Column("heure_debut")]
        public TimeSpan HeureDebut { get; set; }

        [Required]
        [Column("heure_fin")]
        public TimeSpan HeureFin { get; set; }

        [Required]
        [Column("motif")]
        public string Motif { get; set; }

        [Required]
        [MaxLength(15)]
        [Column("statut")]
        public string Statut { get; set; } = "en_attente";

        [Column("id_salle")]
        public int IdSalle { get; set; }

        [ForeignKey("IdSalle")]
        public virtual Salle Salle { get; set; }

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Column("id_club")]
        public int? IdClub { get; set; }

        [ForeignKey("IdClub")]
        public virtual Club Club { get; set; }
    }
}