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
        public DateTime DateCreated { get; set; }
    }
}
