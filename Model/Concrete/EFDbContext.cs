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

        public DbSet<Festival> Festivals { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Performance> Performances { get; set;}
        
        public DbSet<Stage> Stages { get; set; }
        
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}