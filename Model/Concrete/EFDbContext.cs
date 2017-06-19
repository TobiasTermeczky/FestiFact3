using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Model.Models;

namespace Model.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("EFDbContext")
        {

        }
    
        public DbSet<Artist> Artist { get; set; }

        public DbSet<Festival> Festival { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}