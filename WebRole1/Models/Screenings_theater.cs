using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Screenings_theater
    {
        public int Id_screening { get; set; }

        public string Schedule { get; set; }

        public int Id_movie { get; set; }

        public string M_name { get; set; }

        public int Id_room { get; set; }

        public string R_name { get; set; }

        public int Id_theater { get; set; }

        public string T_name { get; set; }
    }
}