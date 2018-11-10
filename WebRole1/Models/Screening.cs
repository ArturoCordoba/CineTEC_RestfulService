using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Screening
    {
        public int Id_screening { get; set; }

        public decimal Price { get; set; }

        public string Schedule { get; set; }

        public int Id_room { get; set; }

        public int Id_movie { get; set; }
    }
}