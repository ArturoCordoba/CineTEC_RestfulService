using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class ClientController : Controller
    {
        /// <summary>
        /// Metodo para obtener todos los clientes de la base de datos
        /// </summary>
        /// <returns>
        /// Una lista en formato Json con todos los clientes
        /// </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            //Se abre la conexion entre C# y la base de datos
            SqlConnection connection = new SqlConnection(Connection.connStringTEConstruye);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "SELECT * FROM CLIENT";

            //Se ejecuta el comando
            SqlDataAdapter sqlData = new SqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Client
            {
                F_name = x.Field<string>("F_name"),
                FL_name = x.Field<string>("FL_name"),
                SL_name = x.Field<string>("SL_name"),
                Id_client = x.Field<string>("Id_client"),
                Email = x.Field<string>("Email"),
                Telephone = x.Field<string>("Telephone")
            });

            //Se transforma la informacion obtenida a formato Json
            return Json(new { data = data.ToList() }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Metodo para obtener un cliente especifico
        /// </summary>
        /// <param name="id">Identificador del cliente</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(string id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                SqlConnection connection = new SqlConnection(Connection.connStringTEConstruye);
                connection.Open();

                //Se declara la consulta SQL a ejecutar
                string sqlQuery = "SELECT * FROM CLIENT WHERE Id_client = " + id;

                //Se ejecuta la consulta
                SqlDataAdapter sqlData = new SqlDataAdapter(sqlQuery, connection);

                //Se almacena la respuesta
                DataSet dataSet = new DataSet();
                sqlData.Fill(dataSet);

                //Se cierra la conexion
                connection.Close();

                //Se crea una variable con los objetos recuperados
                var data = dataSet.Tables[0].AsEnumerable().Select(x => new Client
                {
                    F_name = x.Field<string>("F_name"),
                    FL_name = x.Field<string>("FL_name"),
                    SL_name = x.Field<string>("SL_name"),
                    Id_client = x.Field<string>("Id_client"),
                    Email = x.Field<string>("Email"),
                    Telephone = x.Field<string>("Telephone")
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
        /// Metodo para agregar un nuevo cliente
        /// </summary>
        /// <param name="client">Objeto con los datos del cliente a almacenar</param>
        /// <returns>Json con un data = success si la operacion se realizo con exito, data = error en caso contrario</returns>
        [HttpPost]
        public ActionResult Create(Client client)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                SqlConnection connection = new SqlConnection(Connection.connStringTEConstruye);
                connection.Open();

                SqlCommand command = new SqlCommand("INSERT INTO CLIENT VALUES(@F_name, @FL_name, @SL_name, @Id_client, @Email, @Telephone)", connection);
                command.Parameters.Add(new SqlParameter("F_name", client.F_name));
                command.Parameters.Add(new SqlParameter("FL_name", client.FL_name));
                command.Parameters.Add(new SqlParameter("SL_name", client.SL_name));
                command.Parameters.Add(new SqlParameter("Id_client", client.Id_client));
                command.Parameters.Add(new SqlParameter("Email", client.Email));

                if (client.Telephone != null)
                {
                    command.Parameters.Add(new SqlParameter("Telephone", client.Telephone));
                }
                else //Caso en que la variable es nula
                {
                    command.Parameters.Add(new SqlParameter("Telephone", DBNull.Value));
                }

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
        /// Metodo para editar los datos pertenecientes a un cliente especifico
        /// </summary>
        /// <param name="client">Objeto que almacena la informacion del cliente que se desea editar</param>
        /// <returns>
        /// Json con la respuesta obtenida a partir del exito de la operacion
        /// </returns>
        [HttpPut]
        public ActionResult Edit(Client client)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                SqlConnection connection = new SqlConnection(Connection.connStringTEConstruye);
                connection.Open();

                //Se declara el query a ejecutar
                SqlCommand command = new SqlCommand("UPDATE CLIENT SET F_name = @F_name, FL_name = @FL_name, SL_name = @SL_name, Id_client = @Id_client, Email = @Email, Telephone = @Telephone WHERE Id_client = @Id_client", connection);
                command.Parameters.Add(new SqlParameter("F_name", client.F_name));
                command.Parameters.Add(new SqlParameter("FL_name", client.FL_name));
                command.Parameters.Add(new SqlParameter("SL_name", client.SL_name));
                command.Parameters.Add(new SqlParameter("Id_client", client.Id_client));
                command.Parameters.Add(new SqlParameter("Email", client.Email));
                command.Parameters.Add(new SqlParameter("Telephone", client.Telephone));

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
        
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            try
            {
                //Se abre la conexion entre C# y la base de datos
                SqlConnection connection = new SqlConnection(Connection.connStringTEConstruye);
                connection.Open();

                //Se declara el comando SQL a ejecutar
                string sqlQuery = "DELETE FROM CLIENT WHERE Id_client = @Id_client";

                //Se declara el query a ejecutar
                SqlCommand command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("Id_client", id));

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