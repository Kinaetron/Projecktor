using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IHashtagService
    {
        void Create(int postId, int userId, string hashtags);
        void Create(int postId, int userId, Hashtag[] hashtags);
        void Delete(int postId);
        IEnumerable<Hashtag> GetHashTagsFor(int postId);
    }
}
