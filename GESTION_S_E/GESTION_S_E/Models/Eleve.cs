using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("eleves")]
    public class Eleve
    {
        [Key]
        [Column("id_eleve")]
        public int IdEleve { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_eleve")]
        public string NomEleve { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("prenom_eleve")]
        public string PrenomEleve { get; set; } = string.Empty;

        [Column("date_naissance")]
        public DateTime? DateNaissance { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("matricule")]
        public string Matricule { get; set; } = string.Empty;

        [MaxLength(20)]
        [Column("telephone")]
        public string? Telephone { get; set; }

        // --- CLÉS ÉTRANGÈRES ---

        [Column("id_classe")]
        public int IdClasse { get; set; }

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        // Navigation properties
        [ForeignKey("IdClasse")]
        public virtual Classe? Classe { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur? Utilisateur { get; set; }
    }
}