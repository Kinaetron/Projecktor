using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class SearchModel
    {
        private ExternalNavViewModel navigationInfo = new ExternalNavViewModel();
        public ExternalNavViewModel NavigationInfo
        {
            get { return navigationInfo; }
            set { value = navigationInfo; }
        }
        public IEnumerable<User> FoundUsers { get; set; }
        public IEnumerable<PostViewModel> FoundPosts { get; set; }
    }
}