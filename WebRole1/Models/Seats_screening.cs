using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Seats_screening
    {
        public int Id_screening { get; set; }

        public string Number_row { get; set; }

        public int Number_column { get; set; }

        public string M_name { get; set; }

        public string R_name { get; set; }
    }
}