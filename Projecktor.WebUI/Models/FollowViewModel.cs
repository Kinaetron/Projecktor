using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class FollowViewModel
    {
        public IEnumerable<User> FollowData { get; set; }
    }
}