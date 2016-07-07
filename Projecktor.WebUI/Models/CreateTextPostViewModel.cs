using System.ComponentModel.DataAnnotations;

namespace Projecktor.WebUI.Models
{
    public class CreateTextPostViewModel
    {
        [Required]
        [MaxLength(500, ErrorMessage =" Post can't be more than 500 characters")]
        public string TextPost { get; set; }
    }
}