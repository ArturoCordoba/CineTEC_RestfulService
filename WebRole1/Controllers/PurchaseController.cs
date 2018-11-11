using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebRole1.Models;

namespace WebRole1.Controllers
{
    public class PurchaseController : Controller
    {
        [HttpPost]
        public ActionResult Create(Purchase purchase)
        {
            try
            {
                //Se crea una nueva factura y se obtiene el numero respectivo
                int Id_bill = createBill(purchase.Datetime);

                //Se insertan los boletos a la base de datos
                int count_seats = purchase.Seats.Count();
                for (int i = 0; i < count_seats; i++)
                {
                    Seat seat = purchase.Seats[i];
                    insert_tickets(purchase.Id_client, purchase.Id_screening, Id_bill, seat.Number_row, seat.Number_column);
                }

                return Json(new { data = "success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e) //Si ocurrio algun error se retorna un Json indicandolo
            {
                return Json(new { error = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Metodo para crear una factura y obtener su identificador
        /// </summary>
        /// <param name="datetime">Fecha y hora de la compra</param>
        /// <returns>Identificador de una factura</returns>
        private int createBill(string datetime)
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            string sqlQuery = "INSERT INTO BILL (Datetime) VALUES ('" + DateTime.Parse(datetime).ToString("yyyy-MM-dd HH:mm:ss") + "') RETURNING Id_bill";

            //Se ejecuta el comando
            NpgsqlDataAdapter sqlData = new NpgsqlDataAdapter(sqlQuery, connection);

            //Se almacena la respuesta
            DataSet dataSet = new DataSet();
            sqlData.Fill(dataSet);

            //Se cierra la conexion
            connection.Close();

            //Se crea una variable con los objetos recuperados
            var data = dataSet.Tables[0].AsEnumerable().Select(x => new Bill
            {
                Id_bill = x.Field<int>("Id_bill"),
            });

            return data.ToList()[0].Id_bill;
        }

        private void insert_tickets(int Id_client, int Id_screening, int Id_bill, string Number_row, int Number_column)
        {
            //Se abre la conexion entre C# y la base de datos
            NpgsqlConnection connection = new NpgsqlConnection(Connection.connectionString);
            connection.Open();

            //Se declara el comando SQL a ejecutar
            string sqlQuery = "INSERT INTO TICKET (Number_row, Number_column, Id_client, Id_screening, Id_bill) VALUES(@Number_row, @Number_column, @Id_client, @Id_screening, @Id_bill)";

            //Se crea el objeto a cargo de realizar la consulta
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            //Se declara el parametro correspondiente a Number_row
            var paramNumberRow = command.CreateParameter();
            paramNumberRow.ParameterName = "Number_row";
            paramNumberRow.Value = Number_row;
            command.Parameters.Add(paramNumberRow);

            //Se declara el parametro correspondiente a Number_column
            var paramNumberColumn = command.CreateParameter();
            paramNumberColumn.ParameterName = "Number_column";
            paramNumberColumn.Value = Number_column;
            command.Parameters.Add(paramNumberColumn);

            //Se declara el parametro correspondiente a Id_client
            var paramId = command.CreateParameter();
            paramId.ParameterName = "Id_client";
            paramId.Value = Id_client;
            command.Parameters.Add(paramId);

            //Se declara el parametro correspondiente a Id_screening
            var paramIdScreening = command.CreateParameter();
            paramIdScreening.ParameterName = "Id_screening";
            paramIdScreening.Value = Id_screening;
            command.Parameters.Add(paramIdScreening);

            //Se declara el parametro correspondiente a Id_bill
            var paramIdBill = command.CreateParameter();
            paramIdBill.ParameterName = "Id_bill";
            paramIdBill.Value = Id_bill;
            command.Parameters.Add(paramIdBill);

            //Se ejecuta el comando
            command.ExecuteNonQuery();

            //Se cierra la conexion
            connection.Close();
        }
    }
}