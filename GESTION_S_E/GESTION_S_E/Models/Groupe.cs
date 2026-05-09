using GESTION_S_E.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("groupes")]
    public class Groupe
    {
        [Key]
        [Column("id_groupe")]
        public int IdGroupe { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nom_groupe")]
        public string NomGroupe { get; set; }

        // --- RELATION AVEC LA CLASSE ---

        [Column("id_classe")]
        public int IdClasse { get; set; }

        [ForeignKey("IdClasse")]
        public virtual Classe Classe { get; set; }
    }
}
