using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ISecurityService
    {
        bool Authenticate(string username, string password);
        User CreateUser(string username, string password, bool login = true);
        bool DoesUserExist(string username);
        bool IsAuthenticated { get; }
        void Login(User user);
        void Login(string username);
        void Logout();
        int UserId { get; set; }
    }
}
