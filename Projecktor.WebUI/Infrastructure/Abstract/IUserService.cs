using System;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IUserService
    {
        User GetBy(int id);
        User GetBy(string username);
        User GetAllFor(int id);
        User GetAllFor(string username);
        User Create(string username, string password, DateTime? created = null);
    }
}
