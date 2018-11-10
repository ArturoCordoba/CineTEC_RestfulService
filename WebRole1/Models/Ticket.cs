using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Ticket
    {
        public int Id_ticket { get; set; }

        public string Number_row { get; set; }

        public int Number_column { get; set; }

        public string Id_client { get; set; }

        public int Id_screening { get; set; }

        public int Id_bill { get; set; }
    }
}