using System;
using Projecktor.Domain.Entites;


namespace Projecktor.WebUI.Models
{
    public class TextPostViewModel
    {
        public int ReblogId { get; set; }
        public User Reblogger { get; set; }
        public User ReblogedFrom { get; set; }

        public DateTime TimePosted { get; set; }
        public TextPost TextPost { get; set; } 
    }
}