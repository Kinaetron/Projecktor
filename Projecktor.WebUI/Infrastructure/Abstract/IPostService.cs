using System.Collections.Generic;

using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IPostService
    {
        Post Getby(int id);
        Post CreateTextPost(int userId, string status);
        Post CreateTextPost(User user, string status);
        Post CreateImagePost(int userId, string comment, string[] imageLocation);
        Post CreateImagePost(User user, string comment,  string[] imageLocation);
        Post Reblog(int userId, int textId, int reblogId, int sourceId);
        void Delete(int id);
        void DeleteReblog(int id);
        PostViewModel GetPost(int postId);
        IEnumerable<PostViewModel> GetPostsFor(int userId);
        IEnumerable<PostViewModel> GetTimeLineFor(int userId);
        IEnumerable<PostViewModel> GetTagged(string tag);
        IEnumerable<PostViewModel> GetTaggedUser(string tag, string username);
        PostViewModel AssignPost(Post post);
        IEnumerable<Note> Notes(int postId);
    }
}
