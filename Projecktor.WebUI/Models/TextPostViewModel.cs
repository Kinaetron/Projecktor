using System;
using Projecktor.Domain.Entites;


namespace Projecktor.WebUI.Models
{
    public class TextPostViewModel
    {
        public int PostId { get; set; }
        public int TextId { get; set; }
        public int PostCount { get; set; }

        public User Author { get; set; }
        public User ReblogedFrom { get; set; }
        public Post Source { get; set; }
        public string Text { get; set; }

        public DateTime TimePosted { get; set; }
    }
}