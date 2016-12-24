using System.Collections.Generic;

using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ILikeService
    {
        Like Like(int userId, int postId, int sourceId);
        Like Unlike(Like like);
        IEnumerable<PostViewModel> GetLikesFor(int userId);
        void Delete(int postId);
    }
}
