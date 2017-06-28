using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Festival
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime {get; set;}
        public DateTime EndTime { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string OrganizerID { get; set; }
        public int MaxTicket { get; set; }
        public string ImageLink { get; set; }
    }
}