using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class FollowViewModel {
        public int FollowingCount { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<User> FollowData { get; set; }
    }
}