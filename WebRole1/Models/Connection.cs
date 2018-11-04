using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Connection
    {
        //Localhost connection string
        //public static string connectionString = "Server=localhost; Port=5433; User Id=postgres; Password=covicoar; Database=CINETEC;";

        //Azure server connection string
        public static string connectionString = "Server=cinetecdb.postgres.database.azure.com;Database=CINETEC;Port=5432;User Id=cinetec_admin@cinetecdb;Password=cine123tec$;SslMode=Require;";
    }
}
