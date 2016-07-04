using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecktor.Domain.Abstract
{
    public interface IContext : IDisposable
    {
        IUserRepository Users { get; }
        ITextPostRepository TextPosts { get; }

        int SaveChanges();
    }
}
