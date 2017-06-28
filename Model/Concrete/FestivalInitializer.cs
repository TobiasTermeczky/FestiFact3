using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Model.Concrete
{
    public class FestivalInitializer : DropCreateDatabaseAlways<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            //To force the seed
            context.Database.Initialize(true);

            base.InitializeDatabase(context);

            //Festivals
            var festivals = new List<Festival>
            {
                new Festival
                {
                    Name = "Intents Festival",
                    StartTime = new DateTime(2017, 7, 2, 12, 00, 00),
                    EndTime = new DateTime(2017, 7, 3, 00, 00, 00),
                    Price = 49.99m,
                    MaxTicket = 20000,
                    Genre = "Hardstyle"

                },
                new Festival
                {
                    Name = "Defqon.1 Festival",
                    StartTime = new DateTime(2018, 7, 9, 11, 00, 00),
                    EndTime = new DateTime(2018, 7, 9, 23, 00, 00),
                    Price = 49.99m,
                    MaxTicket = 45000,
                    Genre = "Hardstyle"
                }
            };

            festivals.ForEach(x => context.Festivals.AddOrUpdate(x));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}