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

        [Column("nom_salle")]
        [Required(ErrorMessage = "Le nom est requis")]
        public string? NomSalle { get; set; }

        [Column("capacite")]
        [Required(ErrorMessage = "La capacité est requise")]
        public int Capacite { get; set; }

        [Column("type")]
        [Required(ErrorMessage = "Le type est requis")]
        public string? Type { get; set; }

        [Column("localisation")]
        [Required(ErrorMessage = "La localisation est requise")]
        public string? Localisation { get; set; }

        [Column("disponible")]
        public bool Disponible { get; set; }
    }
}