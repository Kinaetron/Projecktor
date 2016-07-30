using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IReblogService
    {
        Reblog Reblog(int rebloggerId, int reblogedFrom ,int postId);
        Reblog Delete(int reblogId);
        IEnumerable<TextPostViewModel> GetReblogsFor(int userId);
    }
}
