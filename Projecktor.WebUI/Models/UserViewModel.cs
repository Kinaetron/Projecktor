using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<string> Texts { get; set; }
    }
}