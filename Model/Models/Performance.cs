using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Performance
    {
        [Key]
        public int Id { get; set; }
        public string Artist { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Stage Stage { get; set; }
    }
}