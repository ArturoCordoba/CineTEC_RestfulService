using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class ActorController : Controller
    {

        /// <summary>
        /// Metodo para obtener todos los actores de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los actores
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM ACTOR";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Actor
            {
                Id_actor = x.Field<int>("Id_actor"),
                F_name = x.Field<string>("F_name"),
                FL_name = x.Field<string>("FL_name"),
                SL_name = x.Field<string>("SL_name")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener un actor especifico a partir de su identificador
        /// </summary>
        /// <param name="id">Id_actor del actor correspondiente</param>
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
                string sqlQuery = "SELECT * FROM ACTOR WHERE Id_actor = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Actor
                {
                    Id_actor = x.Field<int>("Id_actor"),
                    F_name = x.Field<string>("F_name"),
                    FL_name = x.Field<string>("FL_name"),
                    SL_name = x.Field<string>("SL_name")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

            }
            catch //Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo que se encarga de agregar un nuevo actor a la base de datos
        /// </summary>
        /// <param name="actor">Objeto con los datos del actor a insertar</param>
        /// <returns>
        /// Json con un data = success si la operacion se realizo con exito, data = error en caso contrario
        /// </returns>
        [HttpPost]
        public ActionResult Create(Actor actor)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO ACTOR (F_name, FL_name, SL_name) VALUES(@F_name, @FL_name, @SL_name)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a F_name
                var paramFname = command.CreateParameter();
                paramFname.ParameterName = "F_name";
                paramFname.Value = actor.F_name;
                command.Parameters.Add(paramFname);

                //Se declara el parametro correspondiente a FL_name
                var paramFLname = command.CreateParameter();
                paramFLname.ParameterName = "FL_name";
                paramFLname.Value = actor.FL_name;
                command.Parameters.Add(paramFLname);

                //Se declara el parametro correspondiente a SL_name
                var paramSLname = command.CreateParameter();
                paramSLname.ParameterName = "SL_name";
                if (actor.SL_name != null)
                {
                    paramSLname.Value = actor.SL_name;
                }
                else //Caso en que la variable es nula
                {
                    paramSLname.Value = DBNull.Value;
                }
                command.Parameters.Add(paramSLname);

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
                    return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para editar los datos pertenecientes a un actor especifico
        /// </summary>
        /// <param name="actor">Objeto que almacena la informacion del actor que se desea editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Actor actor)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE ACTOR SET F_name = @F_name, FL_name = @FL_name, SL_name = @SL_name WHERE Id_actor = @Id_actor";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_actor
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_actor";
                paramId.Value = actor.Id_actor;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a F_name
                var paramFname = command.CreateParameter();
                paramFname.ParameterName = "F_name";
                paramFname.Value = actor.F_name;
                command.Parameters.Add(paramFname);

                //Se declara el parametro correspondiente a FL_name
                var paramFLname = command.CreateParameter();
                paramFLname.ParameterName = "FL_name";
                paramFLname.Value = actor.FL_name;
                command.Parameters.Add(paramFLname);

                //Se declara el parametro correspondiente a SL_name
                var paramSLname = command.CreateParameter();
                paramSLname.ParameterName = "SL_name";
                if (actor.SL_name != null)
                {
                    paramSLname.Value = actor.SL_name;
                }
                else //Caso en que la variable es nula
                {
                    paramSLname.Value = DBNull.Value;
                }
                command.Parameters.Add(paramSLname);

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
                    return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo que se encarga de eliminar un actor
        /// </summary>
        /// <param name="id">Id del actor que se desea eliminar</param>
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
                string sqlQuery = "DELETE FROM ACTOR WHERE Id_actor = @Id_actor";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_actor
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_actor";
                paramId.Value = id;
                command.Parameters.Add(paramId);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Se retorna un Json indicando que la operacion fue exitosa
                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch //Caso en que ocurrio un error durante el proceso
            {
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
