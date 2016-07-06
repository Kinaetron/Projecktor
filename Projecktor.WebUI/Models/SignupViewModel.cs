using System.ComponentModel.DataAnnotations;


namespace Projecktor.WebUI.Models
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Please enter your desired username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your email addres")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}