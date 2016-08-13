using System;

namespace Projecktor.Domain.Abstract
{
    public interface IContext : IDisposable
    {
        IUserRepository Users { get; }
        IPostRepository Posts { get; }
        ILikeRepository Likes { get; }
        ITextRepository Texts { get; }
        IHashtagRepository Hashtags { get; }

        int SaveChanges();
    }
}
