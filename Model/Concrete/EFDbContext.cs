using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Model.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext()
            : base("EFDbContext")
        {

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}