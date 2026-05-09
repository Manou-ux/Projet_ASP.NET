using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("enseignants")]
    public class Enseignant
    {
        [Key]
        [Column("id_enseignant")]
        public int IdEnseignant { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_enseignant")]
        public string NomEnseignant { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("prenom_enseignant")]
        public string PrenomEnseignant { get; set; }

        [MaxLength(150)]
        [Column("specialite")]
        public string Specialite { get; set; }

        [MaxLength(20)]
        [Column("telephone_enseignant")]
        public string TelephoneEnseignant { get; set; }

        [MaxLength(150)]
        [Column("email_pro")]
        [EmailAddress]
        public string EmailPro { get; set; }

        // --- RELATION ---

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }
    }
}
