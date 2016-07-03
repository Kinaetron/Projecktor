using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projecktor.Domain.Entites
{
    public class TextPost
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }

        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
