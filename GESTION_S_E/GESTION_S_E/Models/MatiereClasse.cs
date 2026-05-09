using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("matiere_classe")]
    public class MatiereClasse
    {
        [Column("id_matiere")]
        public int IdMatiere { get; set; }

        [ForeignKey("IdMatiere")]
        public virtual Matiere Matiere { get; set; }

        [Column("id_classe")]
        public int IdClasse { get; set; }

        [ForeignKey("IdClasse")]
        public virtual Classe Classe { get; set; }
    }
}
