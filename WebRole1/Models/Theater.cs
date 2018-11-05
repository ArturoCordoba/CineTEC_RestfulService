using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Theater
    {
        public int Id_theater { get; set; }

        public string T_name { get; set; }

        public string T_location { get; set; }

        public int Number_rooms { get; set; }
    }
}