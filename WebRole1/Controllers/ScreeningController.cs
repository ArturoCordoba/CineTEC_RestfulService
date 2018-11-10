using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class ScreeningController : Controller
    {
        /// <summary>
        /// Metodo para obtener todas las proyecciones de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los screeninges
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM SCREENING";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Screening
            {
                Id_screening = x.Field<int>("Id_screening"),
                Price = x.Field<decimal>("Price"),
                Schedule = x.Field<DateTime>("Schedule").ToString(),
                Id_room = x.Field<int>("Id_room"),
                Id_movie = x.Field<int>("Id_movie")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener una proyeccion especifica
        /// </summary>
        /// <param name="id">Identificador de la proyeccion</param>
        /// <returns>
        /// Json con la respuesta enviada por el servidor
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
                string sqlQuery = "SELECT * FROM SCREENING WHERE Id_screening = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Screening
                {
                    Id_screening = x.Field<int>("Id_screening"),
                    Price = x.Field<decimal>("Price"),
                    Schedule = x.Field<DateTime>("Schedule").ToString(),
                    Id_room = x.Field<int>("Id_room"),
                    Id_movie = x.Field<int>("Id_movie")
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
        /// Metodo para agregar una nueva proyeccion
        /// </summary>
        /// <param name="screening">Objeto con los datos de la prpyeccion a insertar</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Screening screening)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO screening (Price, Schedule, Id_room, Id_movie) VALUES(@Price, @Schedule, @Id_room, @Id_movie)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Price
                var paramPrice = command.CreateParameter();
                paramPrice.ParameterName = "Price";
                paramPrice.Value = screening.Price;
                command.Parameters.Add(paramPrice);

                //Se declara el parametro correspondiente a Schedule
                var paramSchedule = command.CreateParameter();
                paramSchedule.ParameterName = "Schedule";
                paramSchedule.Value = screening.Schedule;
                command.Parameters.Add(paramSchedule);

                //Se declara el parametro correspondiente a Id_room
                var paramIdRoom = command.CreateParameter();
                paramIdRoom.ParameterName = "Id_room";
                paramIdRoom.Value = screening.Id_room;
                command.Parameters.Add(paramIdRoom);

                //Se declara el parametro correspondiente a Id_movie
                var paramIdMovie = command.CreateParameter();
                paramIdMovie.ParameterName = "Id_movie";
                paramIdMovie.Value = screening.Id_movie;
                command.Parameters.Add(paramIdMovie);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Si la operacion fue exitosa
                if (result > 0)
                {
                    return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
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
        /// Metodo para editar una proyeccion
        /// </summary>
        /// <param name="screening">Objeto que almacena la informacion de la proyeccion que se desea editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Screening screening)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE screening SET Price = @Price, Schedule = @Schedule, Id_room = @Id_room, Id_movie = @Id_movie WHERE Id_screening = @Id_screening";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_screening
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_screening";
                paramId.Value = screening.Id_screening;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a Price
                var paramPrice = command.CreateParameter();
                paramPrice.ParameterName = "Price";
                paramPrice.Value = screening.Price;
                command.Parameters.Add(paramPrice);

                //Se declara el parametro correspondiente a Schedule
                var paramSchedule = command.CreateParameter();
                paramSchedule.ParameterName = "Schedule";
                paramSchedule.Value = DateTime.Parse(screening.Schedule);
                command.Parameters.Add(paramSchedule);

                //Se declara el parametro correspondiente a Id_room
                var paramIdRoom = command.CreateParameter();
                paramIdRoom.ParameterName = "Id_room";
                paramIdRoom.Value = screening.Id_room;
                command.Parameters.Add(paramIdRoom);

                //Se declara el parametro correspondiente a Id_movie
                var paramIdMovie = command.CreateParameter();
                paramIdMovie.ParameterName = "Id_movie";
                paramIdMovie.Value = screening.Id_movie;
                command.Parameters.Add(paramIdMovie);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Si la operacion fue exitosa
                if (result > 0)
                {
                    return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);
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
        /// Metodo para eliminar una proyeccion de la base de datos
        /// </summary>
        /// <param name="id">Identificador de la proyeccion</param>
        /// <returns>
        /// 
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
                string sqlQuery = "DELETE FROM SCREENING WHERE Id_screening = @Id_screening";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_actor
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_screening";
                paramId.Value = id;
                command.Parameters.Add(paramId);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Se retorna un Json indicando que la operacion fue exitosa
                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) //Caso en que ocurrio un error durante el proceso
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}