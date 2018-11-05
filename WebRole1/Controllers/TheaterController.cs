using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class theaterController : Controller
    {
        /// <summary>
        /// Metodo para obtener toda la lista de cines
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los theateres 
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM theater";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Theater
            {
                Id_theater = x.Field<int>("Id_theater"),
                T_name = x.Field<string>("T_name"),
                T_location = x.Field<string>("T_location"),
                Number_rooms = x.Field<int>("Number_rooms")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener un cine en especifico
        /// </summary>
        /// <param name="id">Id del cine</param>
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
                string sqlQuery = "SELECT * FROM theater WHERE Id_theater = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Theater
                {
                    Id_theater = x.Field<int>("Id_theater"),
                    T_name = x.Field<string>("T_name"),
                    T_location = x.Field<string>("T_location"),
                    Number_rooms = x.Field<int>("Number_rooms")
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
        /// Metodo para crear un nuevo cine
        /// </summary>
        /// <param name="theater">Objeto con la informacion del nuevo cine</param>
        /// <returns>
        /// Json con un data = success si la operacion se realizo con exito, data = error en caso contrario
        /// </returns>
        [HttpPost]
        public ActionResult Create(Theater theater)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO theater (T_name, T_location, Number_rooms) VALUES(@T_name, @T_location, @Number_rooms)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a T_name
                var paramTname = command.CreateParameter();
                paramTname.ParameterName = "T_name";
                paramTname.Value = theater.T_name;
                command.Parameters.Add(paramTname);

                //Se declara el parametro correspondiente a T_location
                var paramTlocation = command.CreateParameter();
                paramTlocation.ParameterName = "T_location";
                paramTlocation.Value = theater.T_location;
                command.Parameters.Add(paramTlocation);

                //Se declara el parametro correspondiente a Number_rooms
                var paramNumberRooms = command.CreateParameter();
                paramNumberRooms.ParameterName = "Number_rooms";
                paramNumberRooms.Value = theater.Number_rooms;
                command.Parameters.Add(paramNumberRooms);

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
        /// Metodo para editar la informacion de un cine
        /// </summary>
        /// <param name="theater">Objeto con la informacion del cine para editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Theater theater)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE THEATER SET T_name = @T_name, T_location = @T_location, Number_rooms = @Number_rooms WHERE Id_theater = @Id_theater";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_theater
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_theater";
                paramId.Value = theater.Id_theater;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a T_name
                var paramTname = command.CreateParameter();
                paramTname.ParameterName = "T_name";
                paramTname.Value = theater.T_name;
                command.Parameters.Add(paramTname);

                //Se declara el parametro correspondiente a T_location
                var paramTlocation = command.CreateParameter();
                paramTlocation.ParameterName = "T_location";
                paramTlocation.Value = theater.T_location;
                command.Parameters.Add(paramTlocation);

                //Se declara el parametro correspondiente a Number_rooms
                var paramNumberRooms = command.CreateParameter();
                paramNumberRooms.ParameterName = "Number_rooms";
                paramNumberRooms.Value = theater.Number_rooms;
                command.Parameters.Add(paramNumberRooms);

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
        /// Metodo para eliminar un cine
        /// </summary>
        /// <param name="id">Identificador del cine</param>
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
                string sqlQuery = "DELETE FROM THEATER WHERE Id_theater = @Id_theater";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_theater
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_theater";
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