using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Abstract;

namespace Model.Concrete
{
    public class EFFestivalRepository : IFestivalRepository
    {
        private EFDbContext _dbContext = new EFDbContext();
    }
}