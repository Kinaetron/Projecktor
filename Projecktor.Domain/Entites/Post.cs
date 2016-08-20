using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projecktor.Domain.Entites
{
    public class Post
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
        public int TextId { get; set; }
        public int ReblogId { get; set; }
        public int SourceId { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
        public string Image6 { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
