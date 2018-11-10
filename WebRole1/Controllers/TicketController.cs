using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class TicketController : Controller
    {
        /// <summary>
        /// Metodo para obtener todos los boletos de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los boletos
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM TICKET";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Ticket  
            {
                Id_ticket = x.Field<int>("Id_ticket"),
                Number_row = x.Field<string>("Number_row"),
                Number_column = x.Field<int>("Number_column"),
                Id_client = x.Field<string>("Id_client"),
                Id_screening = x.Field<int>("Id_screening"),
                Id_bill = x.Field<int>("Id_bill")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener un boleto especifico a partir de su identificador
        /// </summary>
        /// <param name="id">Identificador del boleto</param>
        /// <returns>
        /// Json con la respuesta enviada por la base de datos
        /// </returns>
        [HttpGet]
        public ActionResult Get(int id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara la consulta SQL a ejecutar
                string sqlQuery = "SELECT * FROM TICKET WHERE Id_ticket = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Ticket
                {
                    Id_ticket = x.Field<int>("Id_ticket"),
                    Number_row = x.Field<string>("Number_row"),
                    Number_column = x.Field<int>("Number_column"),
                    Id_client = x.Field<string>("Id_client"),
                    Id_screening = x.Field<int>("Id_screening"),
                    Id_bill = x.Field<int>("Id_bill")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)//Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}