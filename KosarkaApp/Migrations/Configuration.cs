namespace KosarkaApp.Migrations
{
    using KosarkaApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KosarkaApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KosarkaApp.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Klubovi.AddOrUpdate(x => x.Id,
                new Klub() { Id = 1, Naziv = "Sacramento Kings", Liga = "NBA", Godina = 1985, Trofeji = 5 },
                new Klub() { Id = 2, Naziv = "Dallas Mavericks", Liga = "NBA", Godina = 1980, Trofeji = 6 },
                new Klub() { Id = 3, Naziv = "Indiana Pacers", Liga = "NBA", Godina = 1967, Trofeji = 13 }
            );

            context.Kosarkasi.AddOrUpdate(x => x.Id,
                new Kosarkas() { Id = 1, Naziv = "Bogdan Bogdanovic", Godina = 1992, BrojUtakmica = 96, ProsecanBrojPoena = 12.3m, KlubId = 1 },
                new Kosarkas() { Id = 2, Naziv = "Luka Doncic", Godina = 1999, BrojUtakmica = 26, ProsecanBrojPoena = 18.2m, KlubId = 2 },
                new Kosarkas() { Id = 3, Naziv = "Bojan Bogdanovic", Godina = 1989, BrojUtakmica = 105, ProsecanBrojPoena = 14.8m, KlubId = 3 },
                new Kosarkas() { Id = 4, Naziv = "Nemanja Bjelica", Godina = 1988, BrojUtakmica = 25, ProsecanBrojPoena = 10.8m, KlubId = 1 }
            );
        }
    }
}
