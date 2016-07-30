using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projecktor.Domain.Entites
{
    public class Reblog
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        public int RebloggerId { get; set; }
        public int ReblogFromdId { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("RebloggerId")]
        public virtual User User { get; set; }
    }
}
