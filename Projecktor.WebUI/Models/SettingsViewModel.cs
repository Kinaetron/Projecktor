using System.ComponentModel.DataAnnotations;

namespace Projecktor.WebUI.Models
{
    public class SettingsViewModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}