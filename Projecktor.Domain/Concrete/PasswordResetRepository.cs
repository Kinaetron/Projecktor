using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using System.Data.Entity;

namespace Projecktor.Domain.Concrete
{
    public class PasswordResetRepository : EfRepository<PasswordReset>, IPasswordResetRepository {
        public PasswordResetRepository(DbContext context, bool sharedContext) : base(context, sharedContext) { }
    }
}

