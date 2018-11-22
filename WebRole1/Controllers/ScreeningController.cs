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
                Schedule = x.Field<DateTime>("Schedule").ToString("yyyy-MM-dd HH:mm:ss"),
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
                    Schedule = x.Field<DateTime>("Schedule").ToString("yyyy-MM-dd HH:mm:ss"),
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

        /// <summary>
        /// Metodo para obtener todas las proyecciones de un cine
        /// </summary>
        /// <param name="id">Identificador de cine</param>
        /// <returns>
        /// Json con la respuesta enviada por la base de datos
        /// </returns>
        [HttpGet]
        public ActionResult GetByTheater(int id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "SELECT * FROM SCREENINGS_THEATER WHERE Id_theater = " + id.ToString();

                //Se ejecuta el comando
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Screenings_theater
                {
                    Id_screening = x.Field<int>("Id_screening"),
                    Schedule = x.Field<DateTime>("Schedule").ToString("yyyy-MM-dd HH:mm:ss"),
                    Id_movie = x.Field<int>("Id_movie"),
                    M_name = x.Field<string>("M_name"),
                    I_name = x.Field<string>("I_name"),
                    Duration = x.Field<decimal>("Duration"),
                    Rating = x.Field<string>("Rating"),
                    Id_room = x.Field<int>("Id_room"),
                    R_name = x.Field<string>("R_name"),
                    Id_theater = x.Field<int>("Id_theater"),
                    T_name = x.Field<string>("T_name")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para obtener los asientos vendidos de una proyeccion
        /// </summary>
        /// <param name="id">Identificador de la proyeccion</param>
        /// <returns>
        /// Json con la respuesta enviada por el servidor
        /// </returns>
        [HttpGet]
        public ActionResult GetSeatsSold(int id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "SELECT * FROM SEATS_SCREENING WHERE Id_screening = " + id.ToString();

                //Se ejecuta el comando
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Seats_screening
                {
                    Id_screening = x.Field<int>("Id_screening"),
                    Number_row = x.Field<string>("Number_row"),
                    Number_column = x.Field<int>("Number_column"),
                    M_name = x.Field<string>("M_name"),
                    R_name = x.Field<string>("R_name")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}