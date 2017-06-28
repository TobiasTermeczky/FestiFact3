using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        public Festival Festival { get; set; }
        [Range(0, 1, ErrorMessage = "Dit moet een waarde zijn tussen 0 & 1")]
        public int Rate { get; set; }
        public string UserId { get; set; }
    }
}