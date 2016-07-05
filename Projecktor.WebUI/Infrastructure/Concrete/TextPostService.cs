using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class TextPostService : ITextPostService
    {
        private readonly IContext context;
        private readonly ITextPostRepository textPosts;

        public TextPostService(IContext context)
        {
            this.context = context;
            textPosts = context.TextPosts;
        }

        public TextPost Getby(int id) {
            return textPosts.GetBy(id);
        }

        public TextPost Create(User user, string text, DateTime? created = null)
        {
            var textPost = new TextPost()
            {
                Text = text,
                DateCreated = created.HasValue ? created.Value : DateTime.Now
            };

            textPosts.Create(textPost);
            context.SaveChanges();

            return textPost;
        }
    }
}