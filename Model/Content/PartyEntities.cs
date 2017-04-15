using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Model.Content
{
    public class PartyEntities : DbContext
    
    {
        public PartyEntities() : base("name=PartyEntities")
        {

        }

        public virtual DbSet<Festival> Festival { get; set; }

    }
}