using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Movie
    {
        public int Id_movie { get; set; }

        public string O_name { get; set; }

        public string M_name { get; set; }

        public string I_name { get; set; }

        public int Number_copies { get; set; }

        public decimal Duration { get; set; }

        public int Id_rating { get; set; }
    }
}