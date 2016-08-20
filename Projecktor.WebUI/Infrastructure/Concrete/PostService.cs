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
        private readonly IHashtagRepository hashtags;

        public PostService(IContext context)
        {
            this.context = context;
            posts = context.Posts;
            texts = context.Texts;
            users = context.Users;
            likes = context.Likes;
            hashtags = context.Hashtags;
        }

        public Post Getby(int id) {
            return posts.GetBy(id);
        }

        public Post CreateTextPost(User user, string blogpost) {
            return CreateTextPost(user.Id, blogpost);
        }

        public Post CreateTextPost(int userId, string blogpost)
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

        public Post CreateImagePost(User user, string comment, string[] imageLocation) {
            return CreateImagePost(user.Id, comment, imageLocation);
        }

        public Post CreateImagePost(int userId, string comment, string[] imageLocation)
        {
            var text = new Text() {
                Post = comment
            };

            texts.Create(text);
            context.SaveChanges();

            Post post = new Post();

            post.AuthorId = userId;
            post.DateCreated = DateTime.Now;
            post.TextId = text.Id;

            post.Image1 = imageLocation[0];
            post.Image2 = imageLocation[1];
            post.Image3 = imageLocation[2];
            post.Image4 = imageLocation[3];
            post.Image5 = imageLocation[4];
            post.Image6 = imageLocation[5];

            posts.Create(post);

            context.SaveChanges();

            return post;
        }

        public Post Reblog(int userId, int textId, int reblogId, int sourceId)
        {
            Post sourcPost = posts.Find(sourceId);

            Post post = new Post();

            post.AuthorId = userId;
            post.TextId = textId;
            post.ReblogId = reblogId;
            post.SourceId = sourceId;
            post.Image1 = sourcPost.Image1;
            post.Image2 = sourcPost.Image2;
            post.Image3 = sourcPost.Image3;
            post.Image4 = sourcPost.Image4;
            post.Image5 = sourcPost.Image5;
            post.Image6 = sourcPost.Image6;
            post.DateCreated = DateTime.Now;

            posts.Create(post);
            context.SaveChanges();
            return post;
        }

        public void Delete(int postId)
        {
            Post post = Getby(postId);

            foreach (var reference in posts.FindAll(p => p.TextId == post.TextId)) {
                posts.Delete(reference);
            }

            texts.Delete(texts.Find(post.TextId));

            posts.Delete(post);
            context.SaveChanges();
        }

        public void DeleteReblog(int postId)
        {
            Post post = Getby(postId);

            posts.Delete(post);
            context.SaveChanges();
        }

        public PostViewModel GetPost(int postId)
        {
            Post post = posts.Find(postId);

            PostViewModel model = new PostViewModel();

            model.TextId = post.TextId;
            model.Author = users.Find(u => u.Id == post.AuthorId);
            model.Text = texts.Find(t => t.Id == post.TextId).Post;
            model.TimePosted = post.DateCreated;
            model.ReblogedFrom = users.Find(u => u.Id == post.ReblogId);
            model.Source = posts.Find(u => u.Id == post.SourceId);

            if (post.Image1 != null) {
                model.Images.Add(post.Image1);
            }
            if (post.Image2 != null) {
                model.Images.Add(post.Image2);
            }
            if (post.Image3 != null) {
                model.Images.Add(post.Image3);
            }
            if (post.Image4 != null) {
                model.Images.Add(post.Image4);
            }
            if (post.Image5 != null) {
                model.Images.Add(post.Image5);
            }
            if (post.Image6 != null) {
                model.Images.Add(post.Image6);
            }

            model.Hashtags = hashtags.FindAll(h => h.PostId == post.Id).ToArray();

            if (post.SourceId > 0) {
                model.PostCount = posts.FindAll(c => c.SourceId == post.SourceId).Count() +
                                  likes.FindAll(l => l.SourceId == post.SourceId).Count();
            }
            else {
                model.PostCount = posts.FindAll(c => c.SourceId == post.Id).Count() +
                                  likes.FindAll(l => l.SourceId == post.Id).Count();
            }

            return model;
        }

        public IEnumerable<PostViewModel> GetPostsFor(int userId)
        {
            List<PostViewModel> modelPosts = new List<PostViewModel>();

            List<Post> userTextPosts = posts.FindAll(p => p.AuthorId == userId).ToList();


            foreach (var p in userTextPosts)
            {
                PostViewModel model = new PostViewModel();

                model.TextId = p.TextId;
                model.Author = users.Find(u => u.Id == p.AuthorId);
                model.Text = texts.Find(t => t.Id == p.TextId).Post;
                model.TimePosted = p.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == p.ReblogId);
                model.Source = posts.Find(u => u.Id == p.SourceId);

                model.Hashtags = hashtags.FindAll(h => h.PostId == p.Id).ToArray();

                if(p.Image1 != null) {
                    model.Images.Add(p.Image1);
                }
                if (p.Image2 != null) {
                    model.Images.Add(p.Image2);
                }
                if (p.Image3 != null) {
                    model.Images.Add(p.Image3);
                }
                if (p.Image4 != null) {
                    model.Images.Add(p.Image4);
                }
                if (p.Image5 != null) {
                    model.Images.Add(p.Image5);
                }
                if (p.Image6 != null) {
                    model.Images.Add(p.Image6);
                }

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

        public IEnumerable<PostViewModel> GetTimeLineFor(int userId)
        {
            List<PostViewModel> timeline = new List<PostViewModel>();

            List<Post> timelinePosts = posts.FindAll(t => t.Author.Followers.Any(f => f.Id == userId) ||
                                                 t.AuthorId == userId).ToList();

            foreach (var p in timelinePosts)
            {
                PostViewModel model = new PostViewModel();

                model.PostId = p.Id;
                model.TextId = p.TextId;
                model.Author = users.Find(u => u.Id == p.AuthorId);
                model.Text = texts.Find(t => t.Id == p.TextId).Post;
                model.TimePosted = p.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == p.ReblogId);
                model.Source = posts.Find(u => u.Id == p.SourceId);

                if (p.Image1 != null) {
                    model.Images.Add(p.Image1);
                }
                if (p.Image2 != null) {
                    model.Images.Add(p.Image2);
                }
                if (p.Image3 != null) {
                    model.Images.Add(p.Image3);
                }
                if (p.Image4 != null) {
                    model.Images.Add(p.Image4);
                }
                if (p.Image5 != null) {
                    model.Images.Add(p.Image5);
                }
                if (p.Image6 != null) {
                    model.Images.Add(p.Image6);
                }

                if (p.SourceId > 0) {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.SourceId).Count() +
                                      likes.FindAll(l => l.SourceId == p.SourceId).Count();
                }
                else {
                    model.PostCount = posts.FindAll(c => c.SourceId == p.Id).Count() +
                                      likes.FindAll(l => l.SourceId == p.Id).Count();
                }

                model.Hashtags = hashtags.FindAll(h => h.PostId == p.Id).ToArray();

                timeline.Add(model);
            }

            return timeline.OrderByDescending(t => t.TimePosted);
        }


        public IEnumerable<PostViewModel> GetTagged(string tag)
        {
            List<PostViewModel> taggedPosts = new List<PostViewModel>();

            List<Hashtag> tags = hashtags.FindAll(h => h.Tag == tag).ToList();

            foreach (var t in tags)
            {
                PostViewModel model = new PostViewModel();

                var tagPost = posts.Find(p => p.Id == t.PostId);

                model.PostId = tagPost.Id;
                model.TextId = tagPost.TextId;
                model.Author = users.Find(u => u.Id == tagPost.AuthorId);
                model.Text = texts.Find(text => text.Id == tagPost.TextId).Post;
                model.TimePosted = tagPost.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == tagPost.ReblogId);
                model.Source = posts.Find(u => u.Id == tagPost.SourceId);

                if (tagPost.Image1 != null) {
                    model.Images.Add(tagPost.Image1);
                }
                if (tagPost.Image2 != null) {
                    model.Images.Add(tagPost.Image2);
                }
                if (tagPost.Image3 != null) {
                    model.Images.Add(tagPost.Image3);
                }
                if (tagPost.Image4 != null) {
                    model.Images.Add(tagPost.Image4);
                }
                if (tagPost.Image5 != null) {
                    model.Images.Add(tagPost.Image5);
                }
                if (tagPost.Image6 != null) {
                    model.Images.Add(tagPost.Image6);
                }

                if (tagPost.SourceId > 0)
                {
                    model.PostCount = posts.FindAll(c => c.SourceId == tagPost.SourceId).Count() +
                                      likes.FindAll(l => l.SourceId == tagPost.SourceId).Count();
                }
                else
                {
                    model.PostCount = posts.FindAll(c => c.SourceId == tagPost.Id).Count() +
                                      likes.FindAll(l => l.SourceId == tagPost.Id).Count();
                }

                model.Hashtags = hashtags.FindAll(h => h.PostId == tagPost.Id).ToArray();

                taggedPosts.Add(model);
            }

            return taggedPosts.OrderByDescending(p => p.TimePosted);
        }

        public IEnumerable<Note> Notes(int postId)
        {
            List<Note> notes = new List<Note>();

            Post sourceTextPost = posts.Find(p => p.Id == postId);

            Note source = new Note {
                Source = sourceTextPost.Author
            };
            notes.Add(source);

             var userTextPosts = posts.FindAll(p => p.SourceId == postId).ToList();

            foreach (var post in userTextPosts)
            {
                Note model = new Note();

                model.Author = users.Find(u => u.Id == post.AuthorId);
                model.ReblogFrom = users.Find(u => u.Id == post.ReblogId);
                model.DateCreated = post.DateCreated;

                notes.Add(model);
            }

            var userLikes = likes.FindAll(l => l.SourceId == postId).ToList();

            foreach (var like in userLikes)
            {
                Note model = new Note();

                model.Author = like.User;
                model.DateCreated = like.DateCreated;

                notes.Add(model);
            }

            return notes.OrderBy(d => d.DateCreated);
        }
    }
}