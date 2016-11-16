using System;

namespace Projecktor.Domain.Entites
{
    public class Follow
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime DateDone { get; set; }
    }
}
