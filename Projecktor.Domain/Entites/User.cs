﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

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

        private ICollection<Like> likes;
        public virtual ICollection<Like> Likes
        {
            get { return likes ?? (likes = new Collection<Like>()); }
            set { likes = value; }
        }

        private ICollection<Reblog> reblogs;
        public virtual ICollection<Reblog> Reblogs
        {
            get { return reblogs ?? (reblogs = new Collection<Reblog>()); }
            set { reblogs = value; }
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
