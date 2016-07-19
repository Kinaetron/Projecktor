using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IReblogService
    {
        Reblog Reblog(int userId, int postId);
        Reblog Delete(Reblog reblog);
        IEnumerable<TextPostViewModel> GetReblogsFor(int userId);
    }
}
