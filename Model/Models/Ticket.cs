using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public Festival Festival { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime BuyDateTime { get; set; }
    }
}