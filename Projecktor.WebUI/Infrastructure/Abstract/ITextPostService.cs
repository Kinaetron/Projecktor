using Projecktor.Domain.Entites;
using System;
using System.Collections.Generic;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ITextPostService
    {
        TextPost Getby(int id);
        TextPost Create(int userId, string status, DateTime? created = null);
        TextPost Create(User user, string status, DateTime? created = null);
        void Delete(int id);
        IEnumerable<TextPost> GetTimeLineFor(int userId);
    }
}
