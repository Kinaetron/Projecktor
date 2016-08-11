using System;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Models
{
    public class Note
    {
        public User Source { get; set; }
        public User Author { get; set; }
        public User ReblogFrom { get; set; }
        public DateTime DateCreated { get; set; }
    }
}