using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class ExternalViewModel
    {
        public string BlogTitle { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}