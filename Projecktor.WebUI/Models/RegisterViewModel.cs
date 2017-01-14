using System.ComponentModel.DataAnnotations;


namespace Projecktor.WebUI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your desired username.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 10 character in length.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Username must be between 3 and 15 character in length.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your email addres")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}