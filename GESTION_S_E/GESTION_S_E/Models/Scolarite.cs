using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("scolarites")]
    public class Scolarite
    {
        [Key]
        [Column("id_scolarite")]
        public int IdScolarite { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_scolarite")]
        public string NomScolarite { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("prenom_scolarite")]
        public string PrenomScolarite { get; set; }

        [MaxLength(100)]
        [Column("fonction")]
        public string Fonction { get; set; }

        [MaxLength(20)]
        [Column("telephone")]
        public string Telephone { get; set; }

        [MaxLength(50)]
        [Column("bureau")]
        public string Bureau { get; set; }

        // --- RELATION ---

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }
    }
}
