using System.ComponentModel.DataAnnotations;

namespace Projecktor.WebUI.Models
{
    public class PasswordRestViewModel
    {
        public int userId { get; set; }
        public int passwordId { get; set; }

        [Required(ErrorMessage = "Please enter your new password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        public string ConfirmPassword { get; set; }
    }
}