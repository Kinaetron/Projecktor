using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projecktor.WebUI.Models
{
    public class CreateImagePostViewModel
    {
        [Required]
        [MaxLength(500, ErrorMessage = "Text can't be more than 500 characters")]
        public string Text { get; set; }
        public string Hashtags { get; set; }

        [DataType(DataType.Upload)]
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }
}