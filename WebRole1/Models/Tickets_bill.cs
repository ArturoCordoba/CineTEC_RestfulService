using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Tickets_bill
    {
        public int Id_bill { get; set; }

        public int Id_ticket { get; set; }

        public string Number_row { get; set; }

        public int Number_column { get; set; }
    }
}