using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class LikeService : ILikeService
    {
        private readonly IContext context;
        private readonly ILikeRepository likes;
        private readonly IPostRepository posts;
        private readonly IUserRepository users;
        private readonly ITextRepository texts;
        private readonly IHashtagRepository hashtags;

        public LikeService(IContext context)
        {
            this.context = context;
            likes = context.Likes;
            posts = context.Posts;
            users = context.Users;
            texts = context.Texts;
            hashtags = context.Hashtags;
        }

        public Like Like(int userId, int postId, int sourceId)
        {
            var like = new Like()
            {
                UserId = userId,
                PostId = postId,
                SourceId = sourceId,
                DateCreated = DateTime.Now
            };

            likes.Create(like);
            context.SaveChanges();

            return like;
        }

        public Like Unlike(Like like)
        {
            likes.Delete(like);
            context.SaveChanges();

            return like;
        }

        public IEnumerable<PostViewModel> GetLikesFor(int userId)
        {
            var userLikes = likes.FindAll(l => l.UserId == userId).ToList();

            List<PostViewModel> postLikes = new List<PostViewModel>();

            foreach (Like like in userLikes)
            {
                PostViewModel model = new PostViewModel();
                Post details = posts.Find(like.PostId);

                model.PostId = like.PostId;
                model.TextId = details.TextId;
                model.Author = users.Find(u => u.Id == details.AuthorId);
                model.Text = texts.Find(t => t.Id == details.TextId).Post;
                model.TimePosted = like.DateCreated;
                model.ReblogedFrom = users.Find(u => u.Id == details.ReblogId);
                model.Source = posts.Find(u => u.Id == details.SourceId);

                model.Hashtags = hashtags.FindAll(h => h.PostId == details.Id).ToArray();

                if (details.Image1 != null) {
                    model.Images.Add(details.Image1);
                }
                if (details.Image2 != null) {
                    model.Images.Add(details.Image2);
                }
                if (details.Image3 != null) {
                    model.Images.Add(details.Image3);
                }
                if (details.Image4 != null) {
                    model.Images.Add(details.Image4);
                }
                if (details.Image5 != null) {
                    model.Images.Add(details.Image5);
                }
                if (details.Image6 != null) {
                    model.Images.Add(details.Image6);
                }

                if (like.SourceId > 0) {
                    model.PostCount = posts.FindAll(c => c.SourceId == like.SourceId).Count() +
                                      likes.FindAll(l => l.SourceId == like.SourceId).Count();
                }
                else {
                    model.PostCount = posts.FindAll(c => c.SourceId == like.Id).Count() +
                                      likes.FindAll(l => l.SourceId == like.Id).Count();
                }

                postLikes.Add(model);
            }

            return postLikes.OrderByDescending(p => p.TimePosted);
        }
    }
}