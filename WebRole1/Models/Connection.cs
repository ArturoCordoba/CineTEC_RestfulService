using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole1.Models
{
    public class Connection
    {
        //Azure POSTGRESQL connection string
        public static string connectionString = "Server=cinetecdb.postgres.database.azure.com;Database=CINETEC;Port=5432;User Id=cinetec_admin@cinetecdb;Password=cine123tec$;SslMode=Require;";

        //Azure SQL Database connection string
        public static string connStringTEConstruye = "Server=tcp:tecontruye.database.windows.net,1433;Initial Catalog=TEConstruye;Persist Security Info=False;User ID=teconstruye_admin;Password=construye123tec$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}
