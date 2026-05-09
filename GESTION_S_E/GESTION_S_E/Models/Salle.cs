using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("salles")]
    public class Salle
    {
        [Key]
        [Column("id_salle")]
        public int IdSalle { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nom_salle")]
        public string NomSalle { get; set; }

        [Required]
        [Column("capacite")]
        public int Capacite { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("type")]
        public string Type { get; set; } = "cours";

        [MaxLength(100)]
        [Column("localisation")]
        public string Localisation { get; set; }

        [Column("disponible")]
        public bool Disponible { get; set; } = true;
    }
}