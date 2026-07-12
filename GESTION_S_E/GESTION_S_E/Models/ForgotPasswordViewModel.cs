using System.ComponentModel.DataAnnotations;

namespace GESTION_S_E.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}