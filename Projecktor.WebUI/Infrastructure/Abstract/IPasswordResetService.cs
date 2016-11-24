
namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IPasswordResetService
    {
        int Create(int userId);
        void Delete(int passwordId);
        bool Exist(int id);
    }
}