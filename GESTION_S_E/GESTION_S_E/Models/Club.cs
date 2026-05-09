using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("clubs")]
    public class Club
    {
        [Key]
        [Column("id_club")]
        public int IdClub { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_club")]
        public string NomClub { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("id_responsable")]
        public int IdResponsable { get; set; }

        [ForeignKey("IdResponsable")]
        public virtual Utilisateur Responsable { get; set; }

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Column("actif")]
        public bool Actif { get; set; } = true;
    }
}