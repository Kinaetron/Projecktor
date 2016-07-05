using System;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IUserService
    {
        User GetBy(int id);
        User GetBy(string username);
        User Create(string username, string password, DateTime? created = null);
    }
}
