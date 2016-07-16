using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class LikeService : ILikeService
    {
        private readonly IContext context;
        private readonly ITextPostRepository textPosts;
        private readonly ILikeRepository likes;

        public LikeService(IContext context)
        {
            this.context = context;
            likes = context.Likes;
            textPosts = context.TextPosts;
        }

        public Like Like(int userId, int postId)
        {
            var like = new Like()
            {
                UserId = userId,
                PostId = postId,
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

        public IEnumerable<TextPost> GetLikesFor(int userId)
        {
            var userLikes = likes.FindAll(l => l.UserId == userId).ToList().
                                   OrderByDescending(l => l.Id);

            List<TextPost> localPosts = new List<TextPost>();

            foreach (var l in userLikes) {
               localPosts.AddRange(textPosts.FindAll(t => t.Id == l.PostId));
            }

            return localPosts;
        }
    }
}