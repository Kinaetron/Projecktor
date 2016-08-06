using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class PostService : IPostService
    {
        private readonly IContext context;

        private readonly IPostRepository posts;
        private readonly ITextRepository texts;
        private readonly IUserRepository users;
        private readonly ILikeRepository likes;

        public PostService(IContext context)
        {
            this.context = context;
            posts = context.Posts;
            texts = context.Texts;
            users = context.Users;
            likes = context.Likes;
        }

        public Post Getby(int id) {
            return posts.GetBy(id);
        }

        public Post Create(User user, string blogpost) {
            return Create(user.Id, blogpost);
        }

        public Post Create(int userId, string blogpost)
        {
            var text = new Text() {
                Post = blogpost
            };

            texts.Create(text);

            context.SaveChanges();

            var post = new Post()
            {
                AuthorId = userId,
                DateCreated = DateTime.Now,
                TextId = text.Id,
            };
            posts.Create(post);

            context.SaveChanges();

            return post;
        }

        public Post Reblog(int userId, int textId, int reblogId, int sourceId)
        {
            var post = new Post()
            {
                AuthorId = userId,
                TextId = textId,
                ReblogId = reblogId,
                SourceId = sourceId,
                DateCreated = DateTime.Now
            };

            posts.Create(post);

            context.SaveChanges();

            return post;
        }

        public void Delete(int postId)
        {
            var post = Getby(postId);

            foreach (var reference in posts.FindAll(p => p.TextId == post.TextId)) {
                posts.Delete(reference);
            }

            texts.Delete(texts.Find(post.TextId));

            posts.Delete(post);
            context.SaveChanges();
        }

        public void DeleteReblog(int postId)
        {
            var post = Getby(postId);

            posts.Delete(post);
            context.SaveChanges();
        }

        public TextPostViewModel GetPost(int postId)
        {
            var post = posts.Find(postId);

            TextPostViewModel model = new TextPostViewModel()
            {
                TextId = post.TextId,
                Author = users.Find(u => u.Id == post.AuthorId),
                PostCount = posts.FindAll(c => c.SourceId == post.SourceId && post.SourceId != 0).Count(),
                Text = texts.Find(t => t.Id == post.TextId).Post,
                TimePosted = post.DateCreated,
                ReblogedFrom = users.Find(u => u.Id == post.ReblogId),
                Source = posts.Find(u => u.Id == post.SourceId)
            };

            return model;
        }

        public IEnumerable<TextPostViewModel> GetPostsFor(int userId)
        {
            List<TextPostViewModel> modelPosts = new List<TextPostViewModel>();

            var userTextPosts = posts.FindAll(p => p.AuthorId == userId).ToList();


            foreach (var p in userTextPosts)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.TextId = p.TextId;
                model.Author = users.Find(u => u.Id == p.AuthorId);
                model.Text = texts.Find(t => t.Id == p.TextId).Post;
                model.TimePosted = p.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == p.ReblogId);
                model.Source = posts.Find(u => u.Id == p.SourceId);

                if (p.SourceId > 0) {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.SourceId).Count() +
                                      likes.FindAll(l => l.SourceId == p.SourceId).Count();
                }
                else {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.Id).Count() +
                                      likes.FindAll(l => l.SourceId == p.Id).Count();
                }

                modelPosts.Add(model);
            }

            return modelPosts.OrderByDescending(m => m.TimePosted);
        }

        public IEnumerable<TextPostViewModel> GetTimeLineFor(int userId)
        {
            List<TextPostViewModel> timeline = new List<TextPostViewModel>();

            var timelinePosts = posts.FindAll(t => t.Author.Followers.Any(f => f.Id == userId) ||
                                                 t.AuthorId == userId).ToList();

            foreach (var p in timelinePosts)
            {
                TextPostViewModel model = new TextPostViewModel();

                model.PostId = p.Id;
                model.TextId = p.TextId;
                model.Author = users.Find(u => u.Id == p.AuthorId);
                model.Text = texts.Find(t => t.Id == p.TextId).Post;
                model.TimePosted = p.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == p.ReblogId);
                model.Source = posts.Find(u => u.Id == p.SourceId);

                if(p.SourceId > 0) {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.SourceId).Count() +
                                      likes.FindAll(l => l.SourceId == p.SourceId).Count();
                }
                else {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.Id).Count() +
                                      likes.FindAll(l => l.SourceId == p.Id).Count();
                }

                timeline.Add(model);
            }

            return timeline.OrderByDescending(t => t.TimePosted);
        }
    }
}