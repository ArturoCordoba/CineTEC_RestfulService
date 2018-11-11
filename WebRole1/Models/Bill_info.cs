using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Bill_info
    {
        public int Id_bill { get; set; }

        public string Datetime { get; set; }

        public string T_name { get; set; }

        public string M_name { get; set; }

        public string R_name { get; set; }

        public string Schedule { get; set; }

        public string Id_client { get; set; }

        public decimal Total { get; set; }
    }
}