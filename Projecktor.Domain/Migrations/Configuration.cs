namespace Projecktor.Domain.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Projecktor.Domain.Concrete.ProjecktorDatabase>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Projecktor.Domain.Concrete.ProjecktorDatabase";
        }

        protected override void Seed(Projecktor.Domain.Concrete.ProjecktorDatabase context)
        {
        }
    }
}
