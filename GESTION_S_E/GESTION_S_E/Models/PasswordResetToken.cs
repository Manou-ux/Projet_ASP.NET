using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GESTION_S_E.Models
{
    [Table("password_reset_tokens")]
    public class PasswordResetToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_utilisateur")]
        public int IdUtilisateur { get; set; }

        [ForeignKey("IdUtilisateur")]
        public virtual Utilisateur Utilisateur { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("date_expiration")]
        public DateTime ExpirationDate { get; set; }

        [Column("utilise")]
        public bool Used { get; set; } = false;
    }
}