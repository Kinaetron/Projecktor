using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Models
{
    public class ViewModelPostEx
    {
        private ExternalNavViewModel navigationInfo = new ExternalNavViewModel();
        public ExternalNavViewModel NavigationInfo
        {
            get { return navigationInfo; }
            set { value = navigationInfo; }
        }
        public PostViewModel Post { get; set; }
    }
}