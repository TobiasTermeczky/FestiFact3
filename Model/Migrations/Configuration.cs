namespace Model.Migrations
{
    using Model.Concrete;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public EFDbContext Context;
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Model.Concrete.EFDbContext";
        }

        protected override void Seed(EFDbContext context)
        {
            this.Context = context;
            base.Seed(context);
        }
    }
}
