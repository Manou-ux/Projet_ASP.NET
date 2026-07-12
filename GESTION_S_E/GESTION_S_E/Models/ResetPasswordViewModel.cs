using System.ComponentModel.DataAnnotations;

namespace GESTION_S_E.Models
{
    public class ResetPasswordViewModel
    {
        public string Token { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }
}