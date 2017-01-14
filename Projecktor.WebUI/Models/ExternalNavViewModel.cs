using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;

namespace Projecktor.WebUI.Models
{
    public class ExternalNavViewModel
    {
        public bool LoggedIn { get; set; }
        public User SubdomainUser { get; set; }
    }
}