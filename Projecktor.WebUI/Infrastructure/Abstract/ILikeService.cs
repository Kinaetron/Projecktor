using Projecktor.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ILikeService
    {
        Like Like(int userId, int postId);
        Like Unlike(Like like);
        IEnumerable<TextPost> GetLikesFor(int userId);
    }
}
