using System;

using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class HashtagService : IHashtagService
    {
        private readonly IContext context;
        private readonly IHashtagRepository hashtags;

        public HashtagService(IContext context)
        {
            this.context = context;
            hashtags = context.Hashtags;
        }

        public void Create(int postId, string hashtagString)
        {
            string[] tags = hashtagString.Split(',');

            foreach (string tag in tags)
            {
                string strippedTag = tag.Trim();

                var entry = new Hashtag()
                {
                    PostId = postId,
                    Tag = strippedTag
                };

                hashtags.Create(entry);
                context.SaveChanges();
            }
        }
    }
}