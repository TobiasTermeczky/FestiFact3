﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public Festival Festival { get; set; }

        public string Barcode { get; set; }
    }
}