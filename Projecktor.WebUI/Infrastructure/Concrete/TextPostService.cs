using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class TextPostService : ITextPostService
    {
        private readonly IContext context;
        private readonly ITextPostRepository textPosts;
        private readonly IReblogRepository reblogs;
        private readonly IUserRepository users;

        public TextPostService(IContext context)
        {
            this.context = context;
            textPosts = context.TextPosts;
            reblogs = context.Reblogs;
            users = context.Users;
        }

        public TextPost Getby(int id) {
            return textPosts.GetBy(id);
        }

        public TextPost Create(User user, string text, DateTime? created = null) {
            return Create(user.Id, text, created);
        }

        public TextPost Create(int userId, string text, DateTime? created = null)
        {
            var textPost = new TextPost()
            {
                Text = text,
                AuthorId = userId,
                DateCreated = created.HasValue ? created.Value : DateTime.Now
            };

            textPosts.Create(textPost);
            context.SaveChanges();

            return textPost;
        }

        public void Delete(int postId)
        {
            var textPost = Getby(postId);

            var reblogPosts = reblogs.FindAll(r => r.PostId == postId);

            foreach (var reblogPost in reblogPosts) {
                reblogs.Delete(reblogPost);
            }

            textPosts.Delete(textPost);
            context.SaveChanges();
        }

        public IEnumerable<TextPostViewModel> GetPostsFor(int userId)
        {
            List<TextPostViewModel> posts = new List<TextPostViewModel>();

            var userTextPosts = users.Find(u => u.Id == userId).TextPosts;

            foreach (var post in userTextPosts)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.TextPost = post;
                model.TimePosted = post.DateCreated;

                posts.Add(model);
            }

            var userReblogs = users.Find(u => u.Id == userId).Reblogs;

            foreach (var reblog in userReblogs)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.TextPost = textPosts.Find(t => t.Id == reblog.PostId);
                model.TimePosted = reblog.DateCreated;

                posts.Add(model);
            }

            return posts.OrderByDescending(p => p.TimePosted);
        }

        public IEnumerable<TextPostViewModel> GetTimeLineFor(int userId)
        {
            List<TextPostViewModel> timeline = new List<TextPostViewModel>();

            var textPostData = textPosts.FindAll(t => t.Author.Followers.Any(f => f.Id == userId) ||
                                                 t.AuthorId == userId);

            foreach (var data in textPostData)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.TextPost = data;
                model.TimePosted = data.DateCreated;

                timeline.Add(model);
            }

            var followingList = users.FindAll(u => u.Followers.Any(f => f.Id == userId)).ToList();
            List<Reblog> reblogList = new List<Reblog>();

            reblogList.AddRange(reblogs.FindAll(r => r.RebloggerId == userId));

            foreach (var following in followingList) {
               reblogList.AddRange(reblogs.FindAll(r => r.RebloggerId == following.Id));
            }

            foreach (var reblog in reblogList)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.ReblogId = reblog.Id;
                model.Reblogger = users.Find(reblog.RebloggerId);
                model.ReblogedFrom = users.Find(reblog.ReblogFromdId);
                model.TextPost = textPosts.Find(t => t.Id == reblog.PostId);
                model.TimePosted = reblog.DateCreated;

                timeline.Add(model);
            }

            return timeline.OrderByDescending(t => t.TimePosted);
        }
    }
}