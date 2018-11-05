using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Room
    {
        public int Id_room { get; set; }

        public string R_name { get; set; }

        public int Number_rows { get; set; }

        public int Number_columns { get; set; }

        public int Id_theater { get; set; }
    }
}