using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("classes")]
    public class Classe
    {
        [Key]
        [Column("id_classe")]
        public int IdClasse { get; set; }

        [Column("nom_classe")]
        [Required]
        public string NomClasse { get; set; } = string.Empty;

        [Column("Niveau")]
        [Required]
        public string Niveau { get; set; } = string.Empty;

        [Column("Filiere")]
        [Required]
        public string Filiere { get; set; } = string.Empty;

        [Column("Effectif")]
        public int Effectif { get; set; }

        [Column("annee_academique")]
        [Required]
        public string AnneeAcademique { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Eleve>? Eleves { get; set; }
        public virtual ICollection<Groupe>? Groupes { get; set; }
        public virtual ICollection<MatiereClasse>? MatiereClasses { get; set; }
        public virtual ICollection<EmploiDuTemps>? EmploisDuTemps { get; set; }
    }
}