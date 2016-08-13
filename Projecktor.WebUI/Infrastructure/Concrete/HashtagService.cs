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

        public void Create(int postId, Hashtag[] hashtagArray)
        {
            foreach (var tag in hashtagArray)
            {
                var entry = new Hashtag()
                {
                    PostId = postId,
                    Tag = tag.Tag
                };

                hashtags.Create(entry);
                context.SaveChanges();
            }
        }

        public void Delete(int postId)
        {
            var deleteHashtags = hashtags.FindAll(h => h.PostId == postId);

            foreach (var tag in deleteHashtags) {
                hashtags.Delete(tag);
            }

            context.SaveChanges();
        }

        public IEnumerable<Hashtag> GetHashTagsFor(int postId)
        {
            var getHashtags = hashtags.FindAll(h => h.PostId == postId);
            return getHashtags;
        }
    }
}