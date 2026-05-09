using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("notifications")]
    public class Notification
    {
        [Key]
        [Column("id_notification")]
        public int IdNotification { get; set; }

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Required]
        [Column("message")]
        public string Message { get; set; }

        [Column("date_envoi")]
        public DateTime DateEnvoi { get; set; } = DateTime.Now;

        [Column("lu")]
        public bool Lu { get; set; } = false;
    }
}