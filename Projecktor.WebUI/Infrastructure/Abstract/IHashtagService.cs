using Projecktor.Domain.Entites;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface IHashtagService
    {
        void Create(int postId, string hashtags);
    }
}
