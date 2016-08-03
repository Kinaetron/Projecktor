using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Projecktor.Domain.Entites
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
