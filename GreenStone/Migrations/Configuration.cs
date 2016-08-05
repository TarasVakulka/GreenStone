namespace GreenStone.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GreenStone.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<GreenStone.DataContext.ManagerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GreenStone.DataContext.ManagerContext";
        }

        protected override void Seed(GreenStone.DataContext.ManagerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('Products', RESEED, 16)");
            




        }
    }
}
