
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("eleve_groupe")]
    public class EleveGroupe
    {
        [Column("id_eleve")]
        public int IdEleve { get; set; }

        [ForeignKey("IdEleve")]
        public virtual Eleve Eleve { get; set; }

        [Column("id_groupe")]
        public int IdGroupe { get; set; }

        [ForeignKey("IdGroupe")]
        public virtual Groupe Groupe { get; set; }
    }
}