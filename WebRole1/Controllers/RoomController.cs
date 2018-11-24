using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class RoomController : Controller
    {
        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");

            return resp;
        }

        private ActionResult get(string sqlQuery)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se ejecuta el comando
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Room
                {
                    Id_room = x.Field<int>("Id_room"),
                    R_name = x.Field<string>("R_name"),
                    Number_rows = x.Field<int>("Number_rows"),
                    Number_columns = x.Field<int>("Number_columns"),
                    Id_theater = x.Field<int>("Id_theater")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { records = data.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)//Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
            
        }

        /// <summary>
        /// Metodo para obtener un listado de todas las salas
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todas las salas
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM ROOM";

            return get(sqlQuery);
        }

        /// <summary>
        /// Metodo para obtener una sala especifica
        /// </summary>
        /// <param name="id">Identificador de la sala</param>
        /// <returns>
        /// Json con la respuesta enviada por la base de datos
        /// </returns>
        [HttpGet]
        public ActionResult Get(int id)
        {
            //Se declara la consulta SQL a ejecutar
            string sqlQuery = "SELECT * FROM ROOM WHERE Id_room = " + id.ToString();

            return get(sqlQuery);
        }

        /// <summary>
        /// Metodo para obtener todas las salas de un cine especifico
        /// </summary>
        /// <param name="id">Identificador del cine</param>
        /// <returns>
        /// Json con la respuesta enviada por la base de datos
        /// </returns>
        [HttpGet]
        public ActionResult GetByTheater(int id)
        {
            //Se declara la consulta SQL a ejecutar
            string sqlQuery = "SELECT * FROM ROOM WHERE Id_theater = " + id.ToString();

            return get(sqlQuery);
        }

        /// <summary>
        /// Metodo que se encarga de agregar una nueva sala a un cine
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Room room)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO ROOM (R_name, Number_rows, Number_columns, Id_theater) VALUES(@R_name, @Number_rows, @Number_columns, @Id_theater)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a R_name
                var paramRname = command.CreateParameter();
                paramRname.ParameterName = "R_name";
                paramRname.Value = room.R_name;
                command.Parameters.Add(paramRname);

                //Se declara el parametro correspondiente a Number_rows
                var paramNumberRows = command.CreateParameter();
                paramNumberRows.ParameterName = "Number_rows";
                paramNumberRows.Value = room.Number_rows;
                command.Parameters.Add(paramNumberRows);

                //Se declara el parametro correspondiente a Number_columns
                var paramNumberColumns = command.CreateParameter();
                paramNumberColumns.ParameterName = "Number_columns";
                paramNumberColumns.Value = room.Number_columns;
                command.Parameters.Add(paramNumberColumns);

                //Se declara el parametro correspondiente a Id_theater
                var paramIdTheater = command.CreateParameter();
                paramIdTheater.ParameterName = "Id_theater";
                paramIdTheater.Value = room.Id_theater;
                command.Parameters.Add(paramIdTheater);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Si la operacion fue exitosa
                if (result > 0)
                {
                    return Json(new { records = "success" }, JsonRequestBehavior.AllowGet);
                }
                //En caso de que ocurriera un error
                else
                {
                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para editar los datos de una sala
        /// </summary>
        /// <param name="room">Objeto con la informacion de la sala para editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Room room)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE ROOM SET R_name = @R_name, Number_rows = @Number_rows, Number_columns = @Number_columns, Id_theater = @Id_theater WHERE Id_room = @Id_room";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_room
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_room";
                paramId.Value = room.Id_room;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a R_name
                var paramRname = command.CreateParameter();
                paramRname.ParameterName = "R_name";
                paramRname.Value = room.R_name;
                command.Parameters.Add(paramRname);

                //Se declara el parametro correspondiente a Number_rows
                var paramNumberRows = command.CreateParameter();
                paramNumberRows.ParameterName = "Number_rows";
                paramNumberRows.Value = room.Number_rows;
                command.Parameters.Add(paramNumberRows);

                //Se declara el parametro correspondiente a Number_columns
                var paramNumberColumns = command.CreateParameter();
                paramNumberColumns.ParameterName = "Number_columns";
                paramNumberColumns.Value = room.Number_columns;
                command.Parameters.Add(paramNumberColumns);

                //Se declara el parametro correspondiente a Id_theater
                var paramIdTheater = command.CreateParameter();
                paramIdTheater.ParameterName = "Id_theater";
                paramIdTheater.Value = room.Id_theater;
                command.Parameters.Add(paramIdTheater);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Si la operacion fue exitosa
                if (result > 0)
                {
                    return Json(new { records = "success" }, JsonRequestBehavior.AllowGet);
                }
                //En caso de que ocurriera un error
                else
                {
                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para eliminar una sala
        /// </summary>
        /// <param name="id">Identificador de la sala</param>
        /// <returns>
        /// Json indicando el resultado de la operacion
        /// </returns>
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "DELETE FROM ROOM WHERE Id_room = @Id_room";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_room
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_room";
                paramId.Value = id;
                command.Parameters.Add(paramId);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Se retorna un Json indicando que la operacion fue exitosa
                return Json(new { records = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) //Caso en que ocurrio un error durante el proceso
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}