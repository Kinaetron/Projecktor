using System;
using System.Linq;
using System.Collections.Generic;

using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;

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
        private readonly IFollowRepository follow;

        public PostService(IContext context)
        {
            this.context = context;
            posts = context.Posts;
            texts = context.Texts;
            users = context.Users;
            likes = context.Likes;
            hashtags = context.Hashtags;
            follow = context.Follow;
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

            Post post = new Post()
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
            Text text = new Text() {
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

            foreach (Post reference in posts.FindAll(p => p.TextId == post.TextId)) {
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

        public PostViewModel GetPost(int postId) {
            return AssignPost(posts.Find(postId));
        }

        public IEnumerable<PostViewModel> GetPostsFor(int userId)
        {
            List<PostViewModel> modelPosts = new List<PostViewModel>();

            List<Post> userTextPosts = posts.FindAll(p => p.AuthorId == userId).ToList();

            foreach (var p in userTextPosts)
            {
                PostViewModel model = AssignPost(p);
                modelPosts.Add(model);
            }

            return modelPosts.OrderByDescending(m => m.TimePosted);
        }

        public IEnumerable<PostViewModel> GetTimeLineFor(int userId)
        {
            List<PostViewModel> timeline = new List<PostViewModel>();
            List<Follow> followings = follow.FindAll(f => f.FollowerId == userId).ToList();
            List<Post> timelinePosts = posts.FindAll(t => t.AuthorId == userId).ToList();

            foreach (var follower in followings) {
                timelinePosts.AddRange(posts.FindAll(p => p.AuthorId == follower.FollowingId));
            }

            foreach (var p in timelinePosts)
            {
                PostViewModel model = AssignPost(p);
                timeline.Add(model);
            }

            return timeline.OrderByDescending(t => t.TimePosted);
        }


        public IEnumerable<PostViewModel> GetTagged(string tag)
        {
            List<PostViewModel> taggedPosts = new List<PostViewModel>();
            List<Hashtag> tags = hashtags.FindAll(h => h.Tag == tag).ToList();

            foreach (Hashtag t in tags)
            {
                PostViewModel model = AssignPost(posts.Find(p => p.Id == t.PostId));
                taggedPosts.Add(model);
            }

            return taggedPosts.OrderByDescending(p => p.TimePosted);
        }

        public IEnumerable<string> GetTagTerms(string tag)
        {
            List<Hashtag> tags = hashtags.FindAll(h => h.Tag.StartsWith(tag)).ToList();
            List<string> tagTerms = new List<string>();

            foreach (Hashtag tagger in tags) {
                tagTerms.Add(tagger.Tag);
            }
            return tagTerms;
        }

        public IEnumerable<PostViewModel> GetTaggedUser(string tag, string username)
        {
            User user = users.GetBy(username);

            List<PostViewModel> taggedPosts = new List<PostViewModel>();
            List<Hashtag> tags = hashtags.FindAll(h => h.Tag == tag && h.UserId == user.Id).ToList();

            foreach (Hashtag t in tags)
            {
                PostViewModel model = AssignPost(posts.Find(p => p.Id == t.PostId));
                taggedPosts.Add(model);
            }

            return taggedPosts.OrderByDescending(p => p.TimePosted);
        }

        public PostViewModel AssignPost(Post userPost)
        {
            PostViewModel model = new PostViewModel();

            model.PostId = userPost.Id;
            model.TextId = userPost.TextId;
            model.Author = users.Find(u => u.Id == userPost.AuthorId);
            model.Text = texts.Find(t => t.Id == userPost.TextId).Post;
            model.TimePosted = userPost.DateCreated;
            model.ReblogedFrom = users.Find(u => u.Id == userPost.ReblogId);
            model.Source = posts.Find(u => u.Id == userPost.SourceId);

            if(userPost.SourceId != 0) {
                model.Notes = Notes(userPost.SourceId).ToList();
            }
            else {
                model.Notes = Notes(userPost.Id).ToList();
            }

            if (userPost.Image1 != null) {
                model.Images.Add(userPost.Image1);
            }
            if (userPost.Image2 != null) {
                model.Images.Add(userPost.Image2);
            }
            if (userPost.Image3 != null) {
                model.Images.Add(userPost.Image3);
            }
            if (userPost.Image4 != null) {
                model.Images.Add(userPost.Image4);
            }
            if (userPost.Image5 != null) {
                model.Images.Add(userPost.Image5);
            }
            if (userPost.Image6 != null) {
                model.Images.Add(userPost.Image6);
            }

            model.Hashtags = hashtags.FindAll(h => h.PostId == userPost.Id).ToArray();

            model.PostCount = model.Notes.Count() - 1;

            return model;
        }

        public IEnumerable<Note> Notes(int postId)
        {
            List<Note> notes = new List<Note>();

            Post sourceTextPost = posts.Find(p => p.Id == postId);

            if(sourceTextPost != null)
            {
                Note source = new Note {
                    Source = sourceTextPost.Author
                };
                notes.Add(source);
            }

             var userTextPosts = posts.FindAll(p => p.SourceId == postId).ToList();

            foreach (Post post in userTextPosts)
            {
                Note model = new Note();

                model.Author = users.Find(u => u.Id == post.AuthorId);
                model.ReblogFrom = users.Find(u => u.Id == post.ReblogId);
                model.DateCreated = post.DateCreated;

                notes.Add(model);
            }

            var userLikes = likes.FindAll(l => l.PostId == postId).ToList();

            foreach (Like like in userLikes)
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