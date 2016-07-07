using Projecktor.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ITextPostService
    {
        TextPost Getby(int id);
        TextPost Create(int userId, string status, DateTime? created = null);
        TextPost Create(User user, string status, DateTime? created = null);
        IEnumerable<TextPost> GetTimeLineFor(int userId);
    }
}
