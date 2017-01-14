using System.Collections.Generic;

namespace Projecktor.WebUI.Models
{
    public class ExternalViewModel
    {
        private ExternalNavViewModel navigationInfo = new ExternalNavViewModel();
        public ExternalNavViewModel NavigationInfo
        {
           get { return navigationInfo; }
           set { value = navigationInfo; }
        }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}