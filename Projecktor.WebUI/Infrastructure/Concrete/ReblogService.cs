using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using Projecktor.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class ReblogService : IReblogService
    {
        private readonly IContext context;
        private readonly IReblogRepository reblogs;
        private readonly ITextPostRepository textPosts;

        public ReblogService(IContext context)
        {
            this.context = context;
            reblogs = context.Reblogs;
            textPosts = context.TextPosts;
        }

        public Reblog Reblog(int userId, int postId)
        {
            var reblog = new Reblog()
            {
                UserId = userId,
                PostId = postId,
                DateCreated = DateTime.Now
            };

            reblogs.Create(reblog);
            context.SaveChanges();

            return reblog;
        }

        public Reblog Delete(Reblog reblog)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TextPostViewModel> GetReblogsFor(int userId)
        {
            var userReblogs = reblogs.FindAll(r => r.UserId == userId).ToList().
                                 OrderByDescending(r => r.Id);

            List<TextPostViewModel> reblogPosts = new List<TextPostViewModel>();

            foreach (var r in userReblogs)
            {
                TextPostViewModel data = new TextPostViewModel();

                data.TextPost = textPosts.Find(t => t.Id == r.PostId);
                data.TimePosted = r.DateCreated;

                reblogPosts.Add(data);
            }



            return reblogPosts;
        }
    }
}