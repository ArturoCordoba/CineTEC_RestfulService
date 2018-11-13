using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class BillController : Controller
    {
        /// <summary>
        /// Metodo para obtener todos las facturas de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos las facturas
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM BILL_INFO";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Bill_info
            {
                Id_bill = x.Field<int>("Id_bill"),
                Datetime = x.Field<DateTime>("Datetime").ToString("dd-MM-yyyy HH:mm:ss"),
                T_name = x.Field<string>("T_name"),
                M_name = x.Field<string>("M_name"),
                R_name = x.Field<string>("R_name"),
                Schedule = x.Field<DateTime>("Schedule").ToString("dd-MM-yyyy HH:mm:ss"),
                Id_client = x.Field<string>("Id_client"),
                Total = x.Field<decimal>("Total")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener una factura especifica a partir de su identificador
        /// </summary>
        /// <param name="id">Identificador de la factura</param>
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
                string sqlQuery = "SELECT * FROM BILL_INFO WHERE Id_bill = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Bill_info
                {
                    Id_bill = x.Field<int>("Id_bill"),
                    Datetime = x.Field<DateTime>("Datetime").ToString("dd-MM-yyyy HH:mm:ss"),
                    T_name = x.Field<string>("T_name"),
                    M_name = x.Field<string>("M_name"),
                    R_name = x.Field<string>("R_name"),
                    Schedule = x.Field<DateTime>("Schedule").ToString("dd-MM-yyyy HH:mm:ss"),
                    Id_client = x.Field<string>("Id_client"),
                    Total = x.Field<decimal>("Total")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)//Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para obtener los asientos pertenecientes a una factura
        /// </summary>
        /// <param name="id">Identificador de la factura</param>
        /// <returns>
        /// Json con la respuesta enviada por la base de datos
        /// </returns>
        [HttpGet]
        public ActionResult GetSeats(int id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara la consulta SQL a ejecutar
                string sqlQuery = "SELECT * FROM TICKETS_BILL WHERE Id_bill = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Tickets_bill
                {
                    Id_bill = x.Field<int>("Id_bill"),
                    Id_ticket = x.Field<int>("Id_ticket"),
                    Number_row = x.Field<string>("Number_row"),
                    Number_column = x.Field<int>("Number_column"),
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