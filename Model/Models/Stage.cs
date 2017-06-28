using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Stage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Festival Festival { get; set; }
    }
}