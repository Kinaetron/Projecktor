using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Projecktor.Domain.Entites
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }


        private ICollection<Post> posts;
        public virtual ICollection<Post> Posts
        {
            get { return posts ?? (posts = new Collection<Post>()); }
            set { posts = value; }
        }

        private ICollection<Text> texts;
        public virtual ICollection<Text> Texts
        {
            get { return texts ?? (texts = new Collection<Text>()); }
            set { texts = value; }
        }

        private ICollection<Like> likes;
        public virtual ICollection<Like> Likes
        {
            get { return likes ?? (likes = new Collection<Like>()); }
            set { likes = value; }
        }

        private ICollection<Hashtag> hashtags;
        public virtual ICollection<Hashtag> Hashtags
        {
            get { return hashtags ?? (hashtags = new Collection<Hashtag>()); }
            set { hashtags = value; }
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
            get { return followers ?? (followers = new Collection<User>()); }
            set { followers = value; }
        }
    }
}
