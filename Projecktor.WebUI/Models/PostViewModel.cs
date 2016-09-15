using System;
using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        public int TextId { get; set; }
        public int PostCount { get; set; }

        public User Author { get; set; }
        public User ReblogedFrom { get; set; }
        public Post Source { get; set; }
        public string Text { get; set; }
        public Hashtag[] Hashtags { get; set; }

        private List<string> images = new List<string>();
        public List<string> Images
        {
            get { return images; }
            set { images = value; }
        }
        public DateTime TimePosted { get; set; }
    }
}