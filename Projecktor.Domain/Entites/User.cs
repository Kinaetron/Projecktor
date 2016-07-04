using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Projecktor.Domain.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }


        private ICollection<TextPost> textPosts;
        public virtual ICollection<TextPost> TextPosts
        {
            get { return textPosts ?? (textPosts = new Collection<TextPost>()); }
            set { textPosts = value; }
        }

        private ICollection<User> following;
        public virtual ICollection<User> Following
        {
            get { return following ?? (following = new Collection<User>()); }
            set { following = value; }
        }

        private ICollection<User> followers;
        public virtual ICollection<User> Followers
        {
            get { return following ?? (followers = new Collection<User>()); }
            set { followers = value; }
        }
    }
}
