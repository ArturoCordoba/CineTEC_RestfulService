using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class RatingController : Controller
    {
        /// <summary>
        /// Metodo para obtener todas las clasificaciones
        /// </summary>
        /// <returns>
        /// Json con la respuesta obtenida
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM RATING";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Rating
            {
                Id_rating = x.Field<int>("Id_rating"),
                R_name = x.Field<string>("R_name"),
                Description = x.Field<string>("Description")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { records = data.ToList() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo para obtener la informacion de una clasificacion especifica
        /// </summary>
        /// <param name="id">Id de la clasificacion que se desea obtener</param>
        /// <returns>
        /// Informacion requerida en formato Json
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
                string sqlQuery = "SELECT * FROM RATING WHERE Id_rating = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Rating
                {
                    Id_rating = x.Field<int>("Id_rating"),
                    R_name = x.Field<string>("R_name"),
                    Description = x.Field<string>("Description")
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
        /// Metodo que se encarga de crear una nueva clasificacion
        /// </summary>
        /// <param name="rating">Objeto con la informacion que se desea insertar en la base de datos</param>
        /// <returns>
        /// Json con un data = success si la operacion se realizo con exito, data = error en caso contrario
        /// </returns>
        [HttpPost]
        public ActionResult Create(Rating rating)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO RATING (R_name, Description) VALUES(@R_name, @Description)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a R_name
                var paramRname = command.CreateParameter();
                paramRname.ParameterName = "R_name";
                paramRname.Value = rating.R_name;
                command.Parameters.Add(paramRname);

                //Se declara el parametro correspondiente a Description
                var paramDescription = command.CreateParameter();
                paramDescription.ParameterName = "Description";
                paramDescription.Value = rating.Description;
                command.Parameters.Add(paramDescription);

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
        /// Metodo para editar la informacion de una clasificacion
        /// </summary>
        /// <param name="rating">Objeto con la informacion del rating a editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del resultado de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Rating rating)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE RATING SET R_name = @R_name, Description = @Description WHERE Id_rating = @Id_rating";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_rating
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_rating";
                paramId.Value = rating.Id_rating;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a R_name
                var paramRname = command.CreateParameter();
                paramRname.ParameterName = "R_name";
                paramRname.Value = rating.R_name;
                command.Parameters.Add(paramRname);

                //Se declara el parametro correspondiente a Description
                var paramDescription = command.CreateParameter();
                paramDescription.ParameterName = "Description";
                paramDescription.Value = rating.Description;
                command.Parameters.Add(paramDescription);
                
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
        /// Metodo para eliminar una clasificacion
        /// </summary>
        /// <param name="id">Identificador de la clasificacion que se desea eliminar</param>
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
                string sqlQuery = "DELETE FROM RATING WHERE Id_rating = @Id_rating";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_actor
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_rating";
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