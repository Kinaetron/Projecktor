using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.Domain.Abstract
{
    public interface ITextPostRepository : IRepository<TextPost>
    {
        TextPost GetBy(int id);
        IEnumerable<TextPost> GetFor(User user);
        void AddFor(TextPost textPost, User user);
    }
}
