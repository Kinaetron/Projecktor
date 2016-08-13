using System.Collections.Generic;

using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IPostService
    {
        Post Getby(int id);
        Post Create(int userId, string status);
        Post Create(User user, string status);
        Post Reblog(int userId, int textId, int reblogId, int sourceId);
        void Delete(int id);
        void DeleteReblog(int id);
        TextPostViewModel GetPost(int postId);
        IEnumerable<TextPostViewModel> GetPostsFor(int userId);
        IEnumerable<TextPostViewModel> GetTimeLineFor(int userId);
        IEnumerable<TextPostViewModel> GetTagged(string tag);
        IEnumerable<Note> Notes(int postId);
    }
}
