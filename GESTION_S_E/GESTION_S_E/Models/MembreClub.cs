using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("membres_club")]
    public class MembreClub
    {
        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Column("id_club")]
        public int IdClub { get; set; }

        [ForeignKey("IdClub")]
        public virtual Club Club { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("role_membre")]
        public string RoleMembre { get; set; } = "membre";

        [Column("date_adhesion")]
        public DateTime DateAdhesion { get; set; } = DateTime.Now;
    }
}
