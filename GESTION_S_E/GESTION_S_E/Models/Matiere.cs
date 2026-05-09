using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("matieres")]
    public class Matiere
    {
        [Key]
        [Column("id_matiere")]
        public int IdMatiere { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nom_matiere")]
        public string NomMatiere { get; set; }

        [MaxLength(50)]
        [Column("code_matiere")]
        public string CodeMatiere { get; set; }

        [Column("volume_horaire")]
        public int? VolumeHoraire { get; set; }

        // DECIMAL(3,2) signifie 3 chiffres au total dont 2 après la virgule (ex: 4.50)
        [Column("coefficient", TypeName = "decimal(3,2)")]
        public decimal Coefficient { get; set; } = 1.00m;
    }
}
