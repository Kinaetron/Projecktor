using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class ExternalViewModel
    {
        public User SubdomainUser { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}