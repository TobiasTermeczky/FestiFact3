using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Festival
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime {get; set;}
        public DateTime EndTime { get; set; }
    }
}