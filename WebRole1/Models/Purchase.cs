using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Purchase
    {
        public int Id_client { get; set; }

        public int Id_screening { get; set; }

        public string Datetime { get; set; }

        public List<Seat> Seats { get; set; }
    }
}