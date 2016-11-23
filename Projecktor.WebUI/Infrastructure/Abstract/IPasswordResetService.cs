using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IPasswordResetService
    {
        int Create(int userId);
        void Delete(int passwordId);
    }
}