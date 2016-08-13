using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IHashtagService
    {
        void Create(int postId, string hashtags);
        void Create(int postId, Hashtag[] hashtags);
        void Delete(int postId);
        IEnumerable<Hashtag> GetHashTagsFor(int postId);
    }
}
