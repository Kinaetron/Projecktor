using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ILikeService
    {
        Like Like(int userId, int postId);
        Like Unlike(Like like);
        IEnumerable<TextPost> GetLikesFor(int userId);
    }
}
