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
    public class DirectorController : Controller
    {
        [AcceptVerbs("OPTIONS")]
        public HttpResponseMessage Options()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("Access-Control-Allow-Origin", "*");
            resp.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");

            return resp;
        }

        /// <summary>
        /// Metodo para obtener todos los directores de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los directores
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM DIRECTOR";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Director
            {
                Id_director = x.Field<int>("Id_director"),
                F_name = x.Field<string>("F_name"),
                FL_name = x.Field<string>("FL_name"),
                SL_name = x.Field<string>("SL_name")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { records = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener un director especifico a partir de su identificador
        /// </summary>
        /// <param name="id">Id_director del director correspondiente</param>
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
                string sqlQuery = "SELECT * FROM DIRECTOR WHERE Id_director = " + id.ToString();

                //Se ejecuta la consulta
                NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Director
                {
                    Id_director = x.Field<int>("Id_director"),
                    F_name = x.Field<string>("F_name"),
                    FL_name = x.Field<string>("FL_name"),
                    SL_name = x.Field<string>("SL_name")
                });

                //Se transforma la informacion obtenida a formato Json
                return Json(new { records = data.ToList() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) //Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo que se encarga de agregar un nuevo director a la base de datos
        /// </summary>
        /// <param name="director">Objeto con los datos del director a insertar</param>
        /// <returns>
        /// Json con un data = success si la operacion se realizo con exito, data = error en caso contrario
        /// </returns>
        [HttpPost]
        public ActionResult Create(Director director)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "INSERT INTO DIRECTOR (F_name, FL_name, SL_name) VALUES(@F_name, @FL_name, @SL_name)";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a F_name
                var paramFname = command.CreateParameter();
                paramFname.ParameterName = "F_name";
                paramFname.Value = director.F_name;
                command.Parameters.Add(paramFname);

                //Se declara el parametro correspondiente a FL_name
                var paramFLname = command.CreateParameter();
                paramFLname.ParameterName = "FL_name";
                paramFLname.Value = director.FL_name;
                command.Parameters.Add(paramFLname);

                //Se declara el parametro correspondiente a SL_name
                var paramSLname = command.CreateParameter();
                paramSLname.ParameterName = "SL_name";
                if (director.SL_name != null)
                {
                    paramSLname.Value = director.SL_name;
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
        /// Metodo para editar los datos pertenecientes a un director especifico
        /// </summary>
        /// <param name="director">Objeto que almacena la informacion del director que se desea editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Director director)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "UPDATE DIRECTOR SET F_name = @F_name, FL_name = @FL_name, SL_name = @SL_name WHERE Id_director = @Id_director";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_director
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_director";
                paramId.Value = director.Id_director;
                command.Parameters.Add(paramId);

                //Se declara el parametro correspondiente a F_name
                var paramFname = command.CreateParameter();
                paramFname.ParameterName = "F_name";
                paramFname.Value = director.F_name;
                command.Parameters.Add(paramFname);

                //Se declara el parametro correspondiente a FL_name
                var paramFLname = command.CreateParameter();
                paramFLname.ParameterName = "FL_name";
                paramFLname.Value = director.FL_name;
                command.Parameters.Add(paramFLname);

                //Se declara el parametro correspondiente a SL_name
                var paramSLname = command.CreateParameter();
                paramSLname.ParameterName = "SL_name";
                if (director.SL_name != null)
                {
                    paramSLname.Value = director.SL_name;
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
        /// Metodo que se encarga de eliminar un director
        /// </summary>
        /// <param name="id">Id del director que se desea eliminar</param>
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
                string sqlQuery = "DELETE FROM DIRECTOR WHERE Id_director = @Id_director";

                //Se crea el objeto a cargo de realizar la consulta
                IDbCommand command = connection.CreateCommand();
                command.CommandText = sqlQuery;

                //Se declara el parametro correspondiente a Id_actor
                var paramId = command.CreateParameter();
                paramId.ParameterName = "Id_director";
                paramId.Value = id;
                command.Parameters.Add(paramId);

                //Se ejecuta el comando, se almacena el resultado en una variable
                int result = command.ExecuteNonQuery();

                //Se cierra la conexion
                connection.Close();

                //Se retorna un Json indicando que la operacion fue exitosa
                return Json(new { records = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)//Caso en que ocurrio un error durante el proceso
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}