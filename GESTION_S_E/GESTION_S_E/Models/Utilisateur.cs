
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("utilisateurs")] // Pour que la table s'appelle ainsi dans PostgreSQL
    public class Utilisateur
    {
        [Key]
        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string MotDePasse { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; }

        public bool Actif { get; set; } = true;

        [Column("date_creation")]
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}