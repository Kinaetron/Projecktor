using Projecktor.Domain.Abstract;
using Projecktor.Domain.Entites;
using Projecktor.WebUI.Infrastructure.Abstract;

namespace Projecktor.WebUI.Infrastructure.Concrete
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IContext context;
        private readonly IPasswordResetRepository passwordReset;

        public PasswordResetService(IContext context)
        {
            this.context = context;
            passwordReset = context.PasswordReset;
        }

        public int Create(int userId)
        {
            PasswordReset reset = new PasswordReset() {
                UserId = userId
            };

            passwordReset.Create(reset);
            context.SaveChanges();

           return reset.Id;
        }

        public void Delete(int passwordId)
        {
            PasswordReset reset = passwordReset.Find(p => p.Id == passwordId);
            passwordReset.Delete(reset);
            context.SaveChanges();
        }
    }
}