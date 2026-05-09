
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("classes")] // Nom de la table dans PostgreSQL
    public class Classe
    {
        [Key]
        [Column("id_classe")]
        public int IdClasse { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_classe")]
        public string NomClasse { get; set; }

        [Required]
        [MaxLength(20)]
        public string Niveau { get; set; } // L1, L2, L3, M1, M2

        [Required]
        [MaxLength(100)]
        public string Filiere { get; set; }

        public int Effectif { get; set; } = 0;

        [Required]
        [MaxLength(20)]
        [Column("annee_academique")]
        public string AnneeAcademique { get; set; }

        public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();
    }
}